namespace BuisnessLogic.Api.Exceptions
{
    public class UserAlreadyExistsApiException : ApiException
    {
        public UserAlreadyExistsApiException(string? message = null)
            : base(message)
        { }
    }
}
