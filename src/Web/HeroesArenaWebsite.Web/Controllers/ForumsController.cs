using System.Collections.Generic;
using System.Linq;
using HeroesArenaWebsite.Data.Models.Forum;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels.Forum;
using Microsoft.AspNetCore.Mvc;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class ForumsController : Controller
    {
        private readonly IForumsService forumsService;

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
    }
}
