using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels.Forum;
using HeroesArenaWebsite.Web.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IApplicationUsersService usersService;
        private readonly IUploadService uploadService;
        private readonly IConfiguration configuration;

        public ProfilesController(UserManager<ApplicationUser> usersManager, IApplicationUsersService usersService,  IUploadService uploadService, IConfiguration configuration)
        {
            this.usersManager = usersManager;
            this.usersService = usersService;
            this.uploadService = uploadService;
            this.configuration = configuration;
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = this.usersManager.GetUserId(this.User);
            var connectionString = this.configuration.GetConnectionString("AzureStorageAccountConnectionString");
            var container = this.uploadService.GetBlobContainer(connectionString);

            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = Path.Combine(parsedContentDisposition.FileName.Trim('"'));

            var blockBlob = container.GetBlockBlobReference(filename);

            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());
            await this.usersService.SetProfileImage(userId, blockBlob.Uri);

            return this.RedirectToAction("Detail", "Profiles", new { id = userId });
        }

        [HttpGet]
        [Authorize]
        [Route("Profiles/Deactivate/{userId:Guid}")]
        public async Task<IActionResult> Deactivate(string userId)
        {
            var user = this.usersService.GetById(userId);
            await this.usersService.Deactivate(user);

            return this.RedirectToAction("Index", "Profiles");
        }
    }
}
