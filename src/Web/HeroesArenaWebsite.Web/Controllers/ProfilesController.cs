using HeroesArenaWebsite.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly ApplicationUser usersService;

        public ProfilesController(UserManager<ApplicationUser> usersManager, ApplicationUser usersService)
        {
            this.usersManager = usersManager;
            this.usersService = usersService;
        }

        //public IActionResult Index()
        //{
        //    var profiles = this.usersService
        //}
    }
}
