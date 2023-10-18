namespace Human_Capital_Managment.Area.HR_Area.Controllers
{
    using Human_Capital_Managment.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("HR Area")]
    [Authorize(Roles = "HR")]
    public class BaseHRController : BaseController
    {

    }
}
