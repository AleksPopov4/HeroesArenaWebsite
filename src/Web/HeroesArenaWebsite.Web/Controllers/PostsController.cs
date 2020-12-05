using System;
using System.Linq;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Common.Repositories;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Data.Models.Forum;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels;
using HeroesArenaWebsite.Web.ViewModels.Post;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly IForumsService forumsService;

        private readonly UserManager<ApplicationUser> userManager;

        public PostsController(IPostsService postsService, IForumsService forumsService, UserManager<ApplicationUser> userManager)
        {
            this.postsService = postsService;
            this.forumsService = forumsService;
            this.userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            var post = this.postsService.GetById(id);

            var replies = post.Replies.Select(reply => new PostReplyViewModel
            {
                Id = reply.Id,
                AuthorId = reply.User.Id,
                AuthorName = reply.User.UserName,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                CreatedOn = reply.CreatedOn,
                ReplyContent = reply.Content,
            });

            var model = new PostIndexViewModel
            {
               Id = post.Id,
               AuthorId = post.User.Id,
               AuthorName = post.User.UserName,
               AuthorImageUrl = post.User.ProfileImageUrl,
               AuthorRating = post.User.Rating,
               CreatedOn = post.CreatedOn,
               PostContent = post.Content,
               Replies = replies,
            };

            return this.View(model);
        }

        public IActionResult Create(int forumId)
        {
            var forum = forumsService.GetById(forumId);

            var model = new CreatePostViewModel
            {
                ForumName = forum.Title,
                AuthorName = this.User.Identity.Name,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(CreatePostViewModel model)
        {
            var userId = this.userManager.GetUserId(this.User);

            var user = await this.userManager.FindByIdAsync(userId);

            var forum = this.forumsService.GetById(model.ForumId);

            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                CreatedOn = DateTime.UtcNow,
                User = user,
                Forum = forum,
            };

            await this.postsService.Add(post);

            return this.RedirectToAction("Index", "Posts", post.Id);
        }
    }
}
