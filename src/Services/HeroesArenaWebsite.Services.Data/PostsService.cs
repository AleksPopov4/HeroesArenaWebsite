using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Common.Repositories;
using HeroesArenaWebsite.Data.Models.Forum;
using Microsoft.EntityFrameworkCore;

namespace HeroesArenaWebsite.Services.Data
{
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public PostsService(IDeletableEntityRepository<Post> postsRepository)
        {
            this.postsRepository = postsRepository;
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
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetPostsByForumId(int forumId)
        {
            return this.postsRepository
                .All()
                .Where(post => post.Forum.Id == forumId);
        }

        public Task Add(Post post)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Post post)
        {
            throw new NotImplementedException();
        }

        public Task EditPost(int id, string newContent)
        {
            throw new NotImplementedException();
        }
    }
}
