namespace BookingService.BookingLogic.Exceptions
{
    public class WrongPassengerDataException : BookingException
    {
        public WrongPassengerDataException(string? message = null)
            : base(message)
        { }
    }
}
