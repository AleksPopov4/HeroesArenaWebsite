using System.Linq;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels.Forum;
using HeroesArenaWebsite.Web.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IApplicationUsersService usersService;

        public ProfilesController(UserManager<ApplicationUser> usersManager, IApplicationUsersService usersService)
        {
            this.usersManager = usersManager;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            var profiles = this.usersService.GetAll()
                .OrderByDescending(user => user.Rating)
                .Select(u => new ProfileViewModel
                {
                    Email = u.Email,
                    ProfileImageUrl = u.ProfileImageUrl,
                    UserRating = u.Rating.ToString(),
                    DateJoined = u.CreatedOn,
                    IsActive = u.IsActive,
                });

            var model = new ProfileListingViewModel
            {
                Profiles = profiles,
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Detail(string id)
        {
            var user = this.usersService.GetById(id);
            var userRoles = this.usersManager.GetRolesAsync(user).Result;

            var model = new ProfileViewModel()
            {
                UserId = user.Id,
                Username = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                DateJoined = user.CreatedOn,
                IsActive = user.IsActive,
                IsAdmin = userRoles.Contains("Admin"),
            };

            return this.View(model);
        }

        public IActionResult Deactivate(string userId)
        {
            var user = this.usersService.GetById(userId);
            this.usersService.Deactivate(user);

            return this.RedirectToAction("Index", "Profiles");
        }
    }
}
