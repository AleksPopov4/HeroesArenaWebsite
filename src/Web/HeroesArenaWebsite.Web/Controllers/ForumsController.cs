using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models.Forum;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels.Forum;
using HeroesArenaWebsite.Web.ViewModels.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class ForumsController : Controller
    {
        private readonly IForumsService forumsService;
        private readonly IUploadService uploadService;
        private readonly IConfiguration configuration;

        public ForumsController(IForumsService forumsService, IUploadService uploadService, IConfiguration configuration)
        {
            this.forumsService = forumsService;
            this.uploadService = uploadService;
            this.configuration = configuration;
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
            var noResults = !string.IsNullOrEmpty(searchQuery) && !posts.Any();
            ForumTopicViewModel model;

            if (forum.Posts.Any())
            {
                var postsListing = posts.Select(post => new PostListingViewModel
                {
                    Id = post.Id,
                    AuthorId = post.User.Id,
                    AuthorName = post.User.UserName,
                    AuthorRating = post.User.Rating,// == null ? 0 : post.User.Rating,
                    Title = post.Title,
                    DatePosted = post.CreatedOn.ToString(CultureInfo.InvariantCulture),
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

                return this.View(model);
            }

            model = new ForumTopicViewModel
            {
                Posts = new List<PostListingViewModel>(),
                Forum = this.BuildForumListing(forum),
                SearchQuery = searchQuery,
                EmptySearchResults = noResults,
            };

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

        [Authorize]
        public IActionResult Create()
        {
            var model = new AddForumInputModel();
            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await this.forumsService.DeleteAsync(id);

            return this.RedirectToAction("Index", "Forums");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddForum(AddForumInputModel model)
        {
            string imageUri;

            if (model.ImageUpload != null)
            {
                var blockBlob = await this.PostForumImage(model.ImageUpload);
                imageUri = blockBlob.Uri.AbsoluteUri;
            }
            else
            {
                imageUri = "../img/icons/default-icon.jpg";
            }

            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                CreatedOn = DateTime.UtcNow,
                ImageUrl = imageUri,
            };

            await this.forumsService.Add(forum);
            return this.RedirectToAction("Index", "Forums");
        }

        private async Task<CloudBlockBlob> PostForumImage(IFormFile file)
        {
            var connectionString = this.configuration.GetConnectionString("AzureStorageAccountConnectionString");
            var container = this.uploadService.GetBlobContainer(connectionString);

            var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = Path.Combine(parsedContentDisposition.FileName.Trim('"'));

            var blockBlob = container.GetBlockBlobReference(filename);
            await blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            return blockBlob;
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
                AllPosts = new List<PostListingViewModel>(),
            };
        }
    }
}
