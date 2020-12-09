using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Common.Repositories;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Data.Models.Forum;
using HeroesArenaWebsite.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HeroesArenaWebsite.Services.Data
{
    public class ForumsService : IForumsService
    {
        private readonly IDeletableEntityRepository<Forum> forumsRepository;

        public ForumsService(IDeletableEntityRepository<Forum> forumsRepository)
        {
            this.forumsRepository = forumsRepository;
        }

        public Forum GetById(int id)
        {
            return this.forumsRepository.All()
                .Where(f => f.Id == id)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                //.Include(f => f.Posts)
                //    .ThenInclude(p => p.Replies)
                //        .ThenInclude(r => r.User)
                .FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<Forum> GetAll()
        {
            return this.forumsRepository.All().OrderBy(x => x.Title)
                .Include(forum => forum.Posts);
        }

        public IEnumerable<ApplicationUser> GetAllActiveUsers()
        {
            throw new System.NotImplementedException();
        }

        public Task Create(Forum forum)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int forumId)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new System.NotImplementedException();
        }
    }
}
