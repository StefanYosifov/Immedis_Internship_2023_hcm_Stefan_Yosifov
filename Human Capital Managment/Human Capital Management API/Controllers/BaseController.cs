namespace Human_Capital_Management_API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    [Area("HR Area")]
    [Authorize(Roles="HR")]
    public class BaseController : ControllerBase
    {

    }
}
