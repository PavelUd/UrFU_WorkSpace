using System.Linq.Expressions;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Helpers.Events;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.Interfaces;

namespace UrFU_WorkSpace_API.Services;

public class ReservationService : IEventHandler<WorkspaceDeletedEvent>, IEventHandler<WorkspaceUpdatedEvent>
{
    private readonly IWorkspaceProvider _workspaceProvider;
    private readonly IReservationRepository _repository;
    private readonly ErrorHandler _errorHandler;

    public ReservationService(IReservationRepository repository, IWorkspaceProvider workspaceProvider, ErrorHandler errorHandler)
    {
        _repository = repository;
        _workspaceProvider = workspaceProvider;
        _errorHandler = errorHandler;
    }
    public Result<IEnumerable<Reservation>> GetReservations(Expression<Func<Reservation, bool>> expression)
    {
        var reservations = _repository.FindByCondition(expression).ToList();
        return Result.Ok<IEnumerable<Reservation>>(reservations);
    }

    public Result<int> AddReservation(Reservation reservation, int idUser)
    {
        reservation.IdUser = idUser;
        return ValidateDate(reservation)
            .Then(_ => ValidateTimeFrame(reservation))
            .Then(_ => _workspaceProvider.GetWorkspaceById(reservation.IdWorkspace, false))
            .Then(_ => _repository.Create(reservation));
    }

    public Result<bool> ConfirmReservation(int idReservation, Coordinate userCoordinate, int idUser)
    {
        const int inaccuracy = 100;

        var confirmResult = GetReservationById(idReservation)
            .Then(r => ValidateUser(r, idUser)
                .Then(_ => _workspaceProvider.GetWorkspaceById(r.IdWorkspace, false))
                .Then(w => new Coordinate(w.Latitude, w.Longitude))
            )
            .Then(c => Utils.GetDistance(c, userCoordinate))
            .Then(d => d <= inaccuracy)
            .Then(isConfirm => _repository.Confirm(idReservation, isConfirm));

        return confirmResult;
    }

    public Result<bool> ConfirmReservation(int idReservation)
    {
        const bool isConfirm = true;
        return _repository.Confirm(idReservation, isConfirm);
    }

    public Result<None> DeleteReservation(int idReservation, int idUser)
    {
        return GetReservationById(idReservation)
            .Then(reservation => ValidateDate(reservation)
                .Then(_ => ValidateUser(reservation, idUser))
                .Then(_ => _repository.Delete(reservation)));
    }
    
    public Result<None> Handle(WorkspaceDeletedEvent @event)
    {
        return GetReservations(x => x.IdWorkspace == @event.WorkspaceId)
            .Then(r => _repository.DeleteRange(r));
    }

    public Result<None> Handle(WorkspaceUpdatedEvent @event)
    {
        foreach (var obj in @event.Objects)
            GetReservations(x => x.IdObject == obj.Id)
                .Then(r => _repository.DeleteRange(r));

        foreach (var weekday in @event.Weekdays)
            GetReservations(x => x.TimeEnd > weekday.TimeEnd || x.TimeStart < weekday.TimeStart)
                .Then(r => _repository.DeleteRange(r));

        return Result.Ok();
    }

    private Result<Reservation> GetReservationById(int id)
    {
        var result = _repository.FindByCondition(x => x.Id == id).FirstOrDefault();
        return result ?? Result.Fail<Reservation>(_errorHandler.RenderError(ErrorType.ReservationsNotFound));
    }


    private Result<None> ValidateTimeFrame(Reservation reservation)
    {
        return reservation.TimeStart >= reservation.TimeEnd
            ? Result.Fail<None>(_errorHandler.RenderError(ErrorType.InvalidCancelReservationTime))
            : Result.Ok();
    }

    private Result<None> ValidateUser(Reservation reservation, int idUser)
    {
        return reservation.IdUser != idUser
            ? Result.Fail<None>(_errorHandler.RenderError(ErrorType.IncorrectReservationOwner))
            : Result.Ok();
    }

    private Result<None> ValidateDate(Reservation reservation)
    {
        var now = DateTime.Now;
        var reservationTime = reservation.Date.ToDateTime(reservation.TimeStart);

        return now >= reservationTime.AddHours(-12)
            ? Result.Fail<None>(_errorHandler.RenderError(ErrorType.InvalidCancelReservationTime))
            : Result.Ok();
    }
}