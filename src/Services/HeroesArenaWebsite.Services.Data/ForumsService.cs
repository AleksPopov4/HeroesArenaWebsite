using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Common.Repositories;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Data.Models.Forum;
using Microsoft.EntityFrameworkCore;

namespace HeroesArenaWebsite.Services.Data
{
    public class ForumsService : IForumsService
    {
        private readonly IDeletableEntityRepository<Forum> forumsRepository;
        private readonly IPostsService postsService;

        public ForumsService(IDeletableEntityRepository<Forum> forumsRepository, IPostsService postsService)
        {
            this.forumsRepository = forumsRepository;
            this.postsService = postsService;
        }

        public Forum GetById(int id)
        {
            return this.forumsRepository.All()
                .Where(f => f.Id == id)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.Replies)
                        .ThenInclude(r => r.User)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.Forum)
                .FirstOrDefault();
        }

        public IEnumerable<Forum> GetAll()
        {
            return this.forumsRepository.All().OrderBy(x => x.Title)
                .Include(forum => forum.Posts);
        }

        public IEnumerable<ApplicationUser> GetActiveUsers(int forumId)
        {
            var posts = this.GetById(forumId).Posts;

            if (posts == null || !posts.Any())
            {
                return new List<ApplicationUser>();
            }

            return this.postsService.GetAllUsers(posts);
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            return this.postsService.GetFilteredPosts(searchQuery);
        }

        public IEnumerable<Post> GetFilteredPosts(int forumId, string searchQuery)
        {
            if (forumId == 0)
            {
                return this.postsService.GetFilteredPosts(searchQuery);
            }

            var forum = this.GetById(forumId);

            return string.IsNullOrEmpty(searchQuery)
                ? forum.Posts
                : forum.Posts.Where(post
                    => post.Title.ToLower().Contains(searchQuery.ToLower()) || post.Content.ToLower().Contains(searchQuery.ToLower()));
        }

        public Post GetLatestPost(int forumId)
        {
            var posts = this.GetById(forumId).Posts;

            if (posts != null)
            {
                return this.GetById(forumId).Posts
                    .OrderByDescending(post => post.CreatedOn)
                    .FirstOrDefault();
            }

            return new Post();
        }

        public bool HasRecentPost(int id)
        {
            const int hoursAgo = 12;
            var window = DateTime.Now.AddHours(-hoursAgo);
            return this.GetById(id).Posts.Any(post => post.CreatedOn >= window);
        }

        public async Task Add(Forum forum)
        {
            await this.forumsRepository.AddAsync(forum);
            await this.forumsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var forum = this.GetById(id);
            this.forumsRepository.HardDelete(forum);

            await this.forumsRepository.SaveChangesAsync();
        }

        public async Task SetForumImage(int id, Uri uri)
        {
            var forum = this.GetById(id);
            forum.ImageUrl = uri.AbsoluteUri;

            this.forumsRepository.Update(forum);
            await this.forumsRepository.SaveChangesAsync();
        }

        public async Task UpdateForumTitle(int forumId, string newTitle)
        {
            var forum = this.GetById(forumId);
            forum.Title = newTitle;

            this.forumsRepository.Update(forum);
            await this.forumsRepository.SaveChangesAsync();
        }

        public async Task UpdateForumDescription(int forumId, string newDescription)
        {
            var forum = this.GetById(forumId);
            forum.Description = newDescription;

            this.forumsRepository.Update(forum);
            await this.forumsRepository.SaveChangesAsync();
        }
    }
}
