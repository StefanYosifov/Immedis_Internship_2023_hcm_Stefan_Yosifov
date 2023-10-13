namespace Human_Capital_Managment.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public abstract class BaseController : Controller
    {

    }
}
