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
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly IForumsService forumsService;
        private readonly ApplicationUser userService;

        private readonly UserManager<ApplicationUser> userManager;

        public PostsController(IPostsService postsService, IForumsService forumsService, UserManager<ApplicationUser> userManager, ApplicationUser userService)
        {
            this.postsService = postsService;
            this.forumsService = forumsService;
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Index(int id)
        {
            var post = this.postsService.GetById(id);

            var replies = post.Replies.Select(reply => new PostReplyViewModel
            {
                Id = reply.Id,
                AuthorId = reply.User?.Id,
                AuthorName = reply.User?.UserName,
                AuthorImageUrl = reply.User?.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                CreatedOn = reply.CreatedOn,
                ReplyContent = reply.Content,
                IsAuthorAdmin = this.userManager.GetRolesAsync(post.User).Result.Contains("Admin"),
            }).OrderBy(reply => reply.CreatedOn);

            var model = new PostIndexViewModel
            {
               Id = post.Id,
               Title = post.Title,
               AuthorId = post.User.Id,
               AuthorName = post.User.UserName,
               AuthorImageUrl = post.User.ProfileImageUrl,
               AuthorRating = post.User.Rating,
               CreatedOn = post.CreatedOn,
               PostContent = post.Content,
               ForumId = post.Forum.Id,
               ForumName = post.Forum.Title,
               IsAuthorAdmin = this.userManager.GetRolesAsync(post.User).Result.Contains("Admin"),
               Replies = replies,
            };

            return this.View(model);
        }

        public IActionResult Create(int id)
        {
            var forum = this.forumsService.GetById(id);

            var model = new CreatePostViewModel
            {
                ForumName = forum.Title,
                AuthorName = this.User.Identity.Name,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
            };

            return this.View(model);
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
                IsArchived = false,
            };

            await this.postsService.Add(post);

            return this.RedirectToAction("Index", "Posts", new { id = post.Id });
        }

        public IActionResult Edit(int postId)
        {
            var post = this.postsService.GetById(postId);

            var model = new CreatePostViewModel
            {
                Title = post.Title,
                Content = post.Content,
                CreatedOn = post.CreatedOn,
            };

            return this.View(model);
        }

        public IActionResult Delete(int id)
        {
            var post = this.postsService.GetById(id);
            var model = new DeletePostViewModel
            {
                PostId = post.Id,
                PostAuthor = post.User.UserName,
                PostContent = post.Content,
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            var post = this.postsService.GetById(id);
            this.postsService.Delete(id);

            return this.RedirectToAction("Index", "Forums", new {id = post.Forum.Id});
        }

        public bool IsAuthorAdmin(ApplicationUser user)
        {
            return this.userManager.GetRolesAsync(user)
                .Result.Contains("Admin");
        }
    }
}
