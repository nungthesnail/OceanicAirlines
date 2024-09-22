namespace BookingService.Repository.Exceptions
{
    public class SavingFailedException : Exception
    {
        public SavingFailedException(string? message = null)
            : base(message)
        { }
    }
}
