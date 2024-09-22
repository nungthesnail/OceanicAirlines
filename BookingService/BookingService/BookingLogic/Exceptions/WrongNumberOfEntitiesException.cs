namespace BookingService.BookingLogic.Exceptions
{
    public class WrongNumberOfEntitiesException : BookingException
    {
        public WrongNumberOfEntitiesException(string? message = null)
            : base(message)
        { }
    }
}
