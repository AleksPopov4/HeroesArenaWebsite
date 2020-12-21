using System.Globalization;
using System.Linq;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels;
using HeroesArenaWebsite.Web.ViewModels.Forum;
using HeroesArenaWebsite.Web.ViewModels.Post;
using HeroesArenaWebsite.Web.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class SearchesController : Controller
    {
        private readonly IPostsService postsService;

        public SearchesController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public IActionResult Results(string searchQuery)
        {
            var posts = this.postsService.GetFilteredPosts(searchQuery).ToList();
            var noResults = !string.IsNullOrEmpty(searchQuery) && !posts.Any();

            var postListing = posts.Select(post => new PostListingViewModel
            {
                Id = post.Id,
                Forum = new ForumListingViewModel
                {
                    Id = post.Forum.Id,
                    ImageUrl = post.Forum.ImageUrl,
                    Title = post.Forum.Title,
                    Description = post.Forum.Description,
                },
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.CreatedOn.ToString(CultureInfo.InvariantCulture),
                RepliesCount = post.Replies.Count(),
            }).OrderByDescending(post => post.DatePosted);

            var model = new SearchResultViewModel
            {
                EmptySearchResults = noResults,
                Posts = postListing,
                SearchQuery = searchQuery,
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return this.RedirectToAction("Results", new {searchQuery});
        }
    }
}
