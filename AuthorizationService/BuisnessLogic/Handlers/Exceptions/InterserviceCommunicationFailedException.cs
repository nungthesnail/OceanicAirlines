namespace BuisnessLogic.Handlers.Exceptions
{
    public class InterserviceCommunicationFailedException : Exception
    {
        public InterserviceCommunicationFailedException(string? message = null)
            : base(message)
        { }
    }
}
