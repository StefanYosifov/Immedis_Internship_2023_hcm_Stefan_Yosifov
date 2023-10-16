namespace Human_Capital_Managment.Controllers.Data
{
    using Human_Capital_Management.Services.UserDetails;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels.UserDetailViewModels;

    public class UserDetailsController : BaseController
    {
        private readonly IUserDetailsService service;

        public UserDetailsController(IUserDetailsService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var userDetails = await service.GetUserDetailsViewModelOptions();
            return View(userDetails);
        }

    }
}
