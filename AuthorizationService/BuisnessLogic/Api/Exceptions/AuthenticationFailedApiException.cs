namespace BuisnessLogic.Api.Exceptions
{
    public class AuthenticationFailedApiException : ApiException
    {
        public AuthenticationFailedApiException(string? message = null)
            : base(message)
        { }
    }
}
