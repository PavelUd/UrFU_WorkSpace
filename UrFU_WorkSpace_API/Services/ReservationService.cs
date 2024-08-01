using System.Linq.Expressions;
using UrFU_WorkSpace_API.Dto;
using UrFU_WorkSpace_API.Enums;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository.Interfaces;

namespace UrFU_WorkSpace_API.Services;

public class ReservationService
{
    private readonly IReservationRepository _repository;
    private readonly ErrorHandler _errorHandler;

    public ReservationService(IReservationRepository repository, IWorkspaceProvider workspaceProvider, ErrorHandler errorHandler)
    {
        _repository = repository;
        _errorHandler = errorHandler;
    }
    
    public Result<IEnumerable<Reservation>> GetReservations(Expression<Func<Reservation, bool>> expression)
    {
        var reservations = _repository.FindByCondition(expression);
        return Result.Ok<IEnumerable<Reservation>>(reservations);
    }
    
    public Result<IEnumerable<FullReservation>> CompleteReservations(List<Reservation> reservations)
    {
        var fullReservations = _repository.IncludeFullInfo(reservations);
        return Result.Ok(fullReservations);
    }
    
    public Result<int> AddReservation(Reservation reservation, int idUser)
    {
        reservation.IdUser = idUser;
        return Validate(reservation.TimeStart < reservation.TimeEnd, ErrorType.InvalidReservationTime)
            .Then(_ => Validate(!IsReservationConflict(reservation), ErrorType.ReservationConflict))
            .Then(_ => Validate(HasTimePassed(reservation, 0), ErrorType.InvalidAddReservation))
            .Then(_ => _repository.Create(reservation));
    }

    public Result<bool> ConfirmReservation(int idReservation, Coordinate userCoordinate, int idUser)
    {
        const int inaccuracy = 100;

        var confirmResult = GetReservationById(idReservation).Then(_repository.IncludeFullInfo)
            .Then(r => 
                 Validate(r.IdUser == idUser, ErrorType.IncorrectReservationOwner)
                .Then( _ => Validate(r.IsAvailableToConfirm, ErrorType.ReservationNotAvailable))
                .Then(_ => r.Coordinate)
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
            .Then(reservation => Validate(HasTimePassed(reservation, 12), ErrorType.InvalidCancelReservation)
                .Then(_ => Validate(reservation.IdUser == idUser, ErrorType.IncorrectReservationOwner))
                .Then(_ => _repository.Delete(reservation)));
    }

    public Result<Reservation> GetReservationById(int id)
    {
        var result = _repository.FindByCondition(x => x.Id == id).FirstOrDefault();
        return result ?? Result.Fail<Reservation>(_errorHandler.RenderError(ErrorType.ReservationsNotFound));
    }
    
    
    private static bool HasTimePassed(Reservation reservation, int hours)
    {
        var now = DateTime.Now;
        var reservationTime = reservation.Date.ToDateTime(reservation.TimeStart);

        return now < reservationTime.AddHours(-hours);
    }

    private Result<None> Validate(bool condition, ErrorType errorType)
    {
        return condition ? Result.Ok() : Result.Fail<None>(_errorHandler.RenderError(errorType));
    }
    
    private bool IsReservationConflict(Reservation reservation)
    {
        return _repository.FindByCondition(r => r.IdObject == reservation.IdObject 
                                  && !(r.TimeStart >= reservation.TimeEnd || r.TimeEnd <= reservation.TimeStart) 
                                  && reservation.Id != r.Id).Any();
    }
}