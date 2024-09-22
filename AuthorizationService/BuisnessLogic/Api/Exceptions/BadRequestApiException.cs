namespace BuisnessLogic.Api.Exceptions
{
    public class BadRequestApiException : ApiException
    {
        public BadRequestApiException(string? message = null)
            : base(message)
        { }
    }
}
