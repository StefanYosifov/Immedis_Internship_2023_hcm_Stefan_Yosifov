namespace Human_Capital_Managment.Controllers.Project
{
    using Human_Capital_Management.Services.Project;

    using Microsoft.AspNetCore.Mvc;

    public class ProjectController : BaseController
    {

        private readonly IProjectService service;

        public ProjectController(IProjectService service)
        {
            this.service = service;
        }

        //[HttpGet]
        //public async Task<IActionResult> MyProjects()
        //{
        //    var getProjects = await service.GetAllMyProjects(GetUserId());
        //    return View(getProjects);
        //}

        //public async Task<IActionResult> MyTeamProjects()
        //{
        //    return null;
        //}
    }
}
