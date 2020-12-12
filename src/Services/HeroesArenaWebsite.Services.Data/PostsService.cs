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
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IDeletableEntityRepository<Forum> forumsRepository;

        public PostsService(IDeletableEntityRepository<Post> postsRepository, IDeletableEntityRepository<Forum> forumsRepository)
        {
            this.postsRepository = postsRepository;
            this.forumsRepository = forumsRepository;
        }

        public Post GetById(int id)
        {
            return this.postsRepository
                .All()
            .Where(post => post.Id == id)
            .Include(post => post.User)
            .Include(post => post.Replies)
            .ThenInclude(reply => reply.User)
            .Include(post => post.Forum)
            .FirstOrDefault();
        }

        public IEnumerable<Post> GetAll()
        {
            return this.postsRepository
                .All()
                .Include(post => post.User)
                .Include(post => post.Replies)
                .ThenInclude(reply => reply.User)
                .Include(post => post.Forum);
        }

        // todo: find a better way
        public IEnumerable<Post> GetPostsByUserId(int id)
        {
            return this.postsRepository
                .All()
                .Where(post => post.User.Id == id.ToString());
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            var searchQueryToLower = searchQuery.ToLower();

            return this.postsRepository.All()
                .Include(post => post.Forum)
                .Include(post => post.User)
                .Include(post => post.Replies)
                .Where(post =>
                    post.Title.ToLower().Contains(searchQueryToLower)
                    || post.Content.ToLower().Contains(searchQueryToLower));
        }

        public IEnumerable<Post> GetPostsByForumId(int id)
        {
            return this.forumsRepository.All()
                .First(forum => forum.Id == id)
                .Posts;
        }

        public IEnumerable<ApplicationUser> GetAllUsers(IEnumerable<Post> posts)
        {
           var users = new List<ApplicationUser>();

           foreach (var post in posts)
           {
               users.Add(post.User);

               if (!post.Replies.Any())
               {
                   continue;
               }

               users.AddRange(post.Replies.Select(reply => reply.User));
           }

           return users.Distinct();
        }

        public async Task Add(Post post)
        {
            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public async Task Archive(int id)
        {
            var post = this.GetById(id);
            post.IsArchived = true;
            this.postsRepository.Update(post);

            await this.postsRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var post = this.GetById(id);
            this.postsRepository.Delete(post);

            await this.postsRepository.SaveChangesAsync();
        }

        public async Task EditPost(int id, string newContent)
        {
            var post = this.GetById(id);
            post.Content = newContent;
            this.postsRepository.Update(post);

            await this.postsRepository.SaveChangesAsync();
        }

        public string GetForumImageUrl(int id)
        {
            var post = this.GetById(id);
            return post.Forum.ImageUrl;
        }
    }
}
