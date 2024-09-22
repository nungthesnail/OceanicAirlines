namespace BuisnessLogic.Api.Exceptions
{
    public class UserDoesntExistsApiException : ApiException
    {
        public UserDoesntExistsApiException(string? message = null)
            : base(message)
        { }
    }
}
