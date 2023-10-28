namespace HCM.Controllers
{
    using Common.Constants;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RestSharp;

    [Authorize]
    public abstract class BaseController : Controller
    {
        protected readonly RestClient client = new(ApplicationAPIConstants.API_BASE_URL);
    }
}
