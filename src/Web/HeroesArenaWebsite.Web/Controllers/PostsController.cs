using System;
using System.Linq;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Data.Models.Forum;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels;
using HeroesArenaWebsite.Web.ViewModels.Post;
using HeroesArenaWebsite.Web.ViewModels.Reply;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly IForumsService forumsService;
        //private readonly ApplicationUser userService;

        private readonly UserManager<ApplicationUser> userManager;

        public PostsController(IPostsService postsService, IForumsService forumsService, UserManager<ApplicationUser> userManager)
        {
            this.postsService = postsService;
            this.forumsService = forumsService;
            this.userManager = userManager;
            //this.userService = userService;
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
                IsAuthorAdmin = this.userManager.GetRolesAsync(reply.User).Result.Contains("Administrator"),
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
               IsAuthorAdmin = this.userManager.GetRolesAsync(post.User).Result.Contains("Administrator"),
               Replies = replies,
            };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Create(int id)
        {
            var forum = this.forumsService.GetById(id);

            var model = new CreatePostInputModel
            {
                ForumName = forum.Title,
                AuthorName = this.User.Identity.Name,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(CreatePostInputModel model)
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

        [Authorize]
        public IActionResult Edit(int id)
        {
            var post = this.postsService.GetById(id);

            var model = new EditPostInputModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedOn = post.CreatedOn,
            };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPost(EditPostInputModel model)
        {
            await this.postsService.EditPost(model.Id, model.Title, model.Content);

            //var post = this.postsService.GetById(model.Id);

            return this.RedirectToAction("Index", "Posts", new { id = model.Id });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var post = this.postsService.GetById(id);
            var model = new DeletePostViewModel
            {
                ForumId = post.Forum.Id,
                PostId = post.Id,
                PostAuthor = post.User.UserName,
                PostContent = post.Content,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                CreatedOn = post.CreatedOn,
                IsAuthorAdmin = this.userManager.GetRolesAsync(post.User).Result.Contains("Administrator"),
            };

            return this.View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var post = this.postsService.GetById(id);
            await this.postsService.DeleteAsync(id);

            return this.RedirectToAction("Index", "Forums", new { id = post.Forum.Id });
        }

        public bool IsAuthorAdmin(ApplicationUser user)
        {
            return this.userManager.GetRolesAsync(user)
                .Result.Contains("Administrator");
        }
    }
}
