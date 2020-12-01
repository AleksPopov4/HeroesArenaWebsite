using System.Linq;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels.Forum;
using Microsoft.AspNetCore.Mvc;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class ForumsController : Controller
    {
        private readonly IForumsService forumsService;
        private readonly IPostService postService;

        public ForumsController(IForumsService forumsService)
        {
            this.forumsService = forumsService;
        }

        public IActionResult Index()
        {
            var forums = this.forumsService.GetAll()
                .Select(forum => new ForumListingViewModel
                {
                    Id = forum.Id,
                    Description = forum.Description,
                    Title = forum.Title,
                });

            var model = new ForumIndexViewModel
            {
                ForumList = forums,
            };

            return this.View(model);
        }

        public IActionResult Topic(int id)
        {
            var forum = this.forumsService.GetById(id);
            var posts = this.postService.GetFilteredPosts(id);
            return;
        }
    }
}
