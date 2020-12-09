using System.Globalization;
using System.Linq;
using HeroesArenaWebsite.Data.Models.Forum;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels.Forum;
using HeroesArenaWebsite.Web.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class ForumsController : Controller
    {
        private readonly IForumsService forumsService;
        private readonly IPostsService postService;

        public ForumsController(IForumsService forumsService, IPostsService postService)
        {
            this.forumsService = forumsService;
            this.postService = postService;
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

            var model = new ForumTopicViewModel
            {
                Forum = this.BuildForumListing(forum),
            };

            if (forum.Posts.Any())
            {
                var postsListing = forum.Posts.Select(post => new PostListingViewModel
                {
                    Id = post.Id,
                    AuthorId = post.User.Id,
                    AuthorName = post.User.UserName,
                    AuthorRating = post.User.Rating,// == null ? 0 : post.User.Rating,
                    Title = post.Title,
                    // todo: invariant culture
                    DatePosted = post.CreatedOn.ToString(),
                    RepliesCount = post.Replies.Count(),// == null ? 0 : post.Replies.Count(),
                    Forum = this.BuildForumListing(post),
                })
                    .ToList();

                model = new ForumTopicViewModel
                {
                    Posts = postsListing,
                    Forum = this.BuildForumListing(forum),
                };
            }

            return this.View(model);
        }

        private ForumListingViewModel BuildForumListing(Post post)
        {
            var forum = post.Forum;

            return this.BuildForumListing(forum);
        }

        private ForumListingViewModel BuildForumListing(Forum forum)
        {
            return new ForumListingViewModel
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl,
            };
        }
    }
}
