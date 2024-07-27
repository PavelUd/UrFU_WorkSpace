namespace UrFU_WorkSpace_API.Enums;

public enum ErrorType
{
    ServerError = 17,
    
    WorkspacesNotFound = 0,
    WorkspaceNotFound = 1,
    WorkspaceComponentNotFound = 2,
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
    
    ReservationsNotFound = 11,
    IncorrectReservationOwner = 16,
    InvalidCancelReservationTime = 14,
    InvalidReservationDate = 15
}