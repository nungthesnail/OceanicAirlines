using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace NotificationService.Controllers
{
	public class SendController : Controller
	{
		[Route("/api/v1/send")]
		[HttpPost]
		[Authorize(Roles = "Admin,Microservice")]
		public IActionResult Send()
		{
			return Ok();
		}
	}
}
