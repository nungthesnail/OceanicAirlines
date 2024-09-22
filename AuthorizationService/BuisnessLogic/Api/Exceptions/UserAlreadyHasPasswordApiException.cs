namespace BuisnessLogic.Api.Exceptions
{
    public class UserAlreadyHasPasswordApiException : ApiException
    {
        public UserAlreadyHasPasswordApiException(string? message = null)
            : base(message)
        { }
    }
}
