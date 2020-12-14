using System.Collections;
using System.Collections.Generic;
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
        private readonly IPostsService postsService;

        public ForumsController(IForumsService forumsService, IPostsService postsService)
        {
            this.forumsService = forumsService;
            this.postsService = postsService;
        }

        public IActionResult Index()
        {
            var forums = this.forumsService.GetAll()
                .Select(forum => new ForumListingViewModel
                {
                    Id = forum.Id,
                    Description = forum.Description,
                    Title = forum.Title,
                    NumberOfPosts = forum.Posts?.Count() ?? 0,
                    Latest = this.GetLatestPost(forum.Id) ?? new PostListingViewModel(),
                    NumberOfUsers = this.forumsService.GetActiveUsers(forum.Id).Count(),
                    ImageUrl = forum.ImageUrl,
                    HasRecentPost = this.forumsService.HasRecentPost(forum.Id),
                });

            var forumListingModels = forums as IList<ForumListingViewModel> ?? forums.ToList();

            var model = new ForumIndexViewModel
            {
                ForumList = forumListingModels.OrderBy(forum => forum.Title),
                NumberOfForums = forumListingModels.Count,
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery = "")
        {
            return this.RedirectToAction("Topic", new { id, searchQuery });
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = this.forumsService.GetById(id);
            var posts = this.forumsService.GetFilteredPosts(id, searchQuery).ToList();

            var model = new ForumTopicViewModel
            {
                Forum = this.BuildForumListing(forum),
                SearchQuery = searchQuery,
                EmptySearchResults = noResults,
            };

            if (forum.Posts.Any())
            {
                var postsListing = posts.Select(post => new PostListingViewModel
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
                }).OrderByDescending(post => post.DatePosted)
                    .ToList();

                model = new ForumTopicViewModel
                {
                    Posts = postsListing,
                    Forum = this.BuildForumListing(forum),
                    SearchQuery = searchQuery,
                    EmptySearchResults = noResults,
                };
            }

            return this.View(model);
        }

        public PostListingViewModel GetLatestPost(int forumId)
        {
            var post = this.forumsService.GetLatestPost(forumId);

            if (post != null)
            {
                return new PostListingViewModel
                {
                    AuthorName = post.User != null ? post.User.UserName : string.Empty,
                    DatePosted = post.CreatedOn.ToString(CultureInfo.InvariantCulture),
                    Title = post.Title ?? string.Empty,
                };
            }

            return new PostListingViewModel();
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
