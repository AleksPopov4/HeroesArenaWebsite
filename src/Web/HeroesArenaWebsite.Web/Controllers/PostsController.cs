using System.Linq;
using HeroesArenaWebsite.Data.Common.Repositories;
using HeroesArenaWebsite.Data.Models.Forum;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels;
using HeroesArenaWebsite.Web.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostsService postsService;

        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
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
    }
}
