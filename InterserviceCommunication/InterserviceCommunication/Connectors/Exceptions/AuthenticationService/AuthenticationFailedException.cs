namespace InterserviceCommunication.Connectors.Exceptions.AuthenticationService
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(string? message = null)
            : base(message) 
        { }
    }
}
