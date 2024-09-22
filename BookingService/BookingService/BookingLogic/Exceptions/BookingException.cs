namespace BookingService.BookingLogic.Exceptions
{
    public class BookingException : Exception
    {
        public BookingException(string? message = null)
            : base(message)
        { }
    }
}
