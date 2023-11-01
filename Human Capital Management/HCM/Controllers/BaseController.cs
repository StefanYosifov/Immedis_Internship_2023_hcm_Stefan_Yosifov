namespace HCM.Controllers
{
    using System.Security.Claims;

    using Common.Constants;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RestSharp;

    [Authorize]
    public abstract class BaseController : Controller
    {
        protected readonly RestClient client = new(ApplicationAPIConstants.API_BASE_URL);

        protected string GetAuthenticationClaim()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.Authentication) == null
                ? HttpContext.Request.Headers["Authorization"]
                : HttpContext.User.FindFirstValue(ClaimTypes.Authentication);
        }
    }
}