namespace BookingService.BookingLogic.Exceptions
{
    public class BookingRegistrationException : BookingException
    {
        public BookingRegistrationException(string? message = null)
            : base(message)
        { }
    }
}
