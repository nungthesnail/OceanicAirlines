namespace FrontendService.Services.Login.Exceptions
{
	public class LoginFailedException : Exception
	{
        public LoginFailedException(string? message = null)
            : base(message)
        { }
    }
}
