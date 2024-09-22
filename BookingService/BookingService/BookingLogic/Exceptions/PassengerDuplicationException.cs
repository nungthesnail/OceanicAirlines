namespace BookingService.BookingLogic.Exceptions
{
    public class PassengerDuplicationException : BookingException
    {
        public PassengerDuplicationException(string? message = null)
            : base(message)
        { }
    }
}
