namespace BuisnessLogic.Api.Exceptions
{
    public class UserDoesntHavePasswordApiException : ApiException
    {
        public UserDoesntHavePasswordApiException(string? message = null)
            : base(message)
        { }
    }
}
