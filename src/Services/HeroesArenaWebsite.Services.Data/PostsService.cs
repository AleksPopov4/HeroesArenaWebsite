using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Common.Repositories;
using HeroesArenaWebsite.Data.Models.Forum;

namespace HeroesArenaWebsite.Services.Data
{
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public PostsService(IDeletableEntityRepository<Post> postsRepository)
        {
            postsRepository = this.postsRepository;
        }

        public Task<Post> GetById(int id)
        {
            throw new NotImplementedException();
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
                .Where(f => f.Forum.Id == forumId);
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
