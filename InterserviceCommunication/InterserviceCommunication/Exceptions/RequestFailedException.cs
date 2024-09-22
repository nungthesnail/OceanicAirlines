namespace InterserviceCommunication.Exceptions
{
    public class RequestFailedException : InterserviceCommunicationException
    {
        public RequestFailedException(string? message = null)
            : base(message)
        { }
    }
}
