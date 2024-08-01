namespace UrFU_WorkSpace_API.Enums;

public enum ErrorType
{
    ServerError = 0,
    
    WorkspaceNotFound = 1,
    InvalidSize = 4,
    InvalidPosition = 3,
    TemplateNotFound = 5,
    IncorrectCountWeekdays = 7,
    InvalidDayOfWeek = 6,
    
    UserConflict = 8,
    UserNotFound = 9,
    IncorrectConfirmCode = 10,
    BadAuthRequest = 12,
    InvalidGrantType = 13,
    InvalidAddReservation = 17,
    ReservationsNotFound = 11,
    IncorrectReservationOwner = 16,
    InvalidReservationTime = 14,
    InvalidCancelReservation = 15,
    ReservationConflict = 18,
    ReservationNotAvailable = 19
}