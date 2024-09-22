namespace BuisnessLogic.Api.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException(string? message = null)
            :base(message)
        { }
    }
}
