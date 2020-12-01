using System.Collections.Generic;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Data.Models.Forum;

namespace HeroesArenaWebsite.Services.Data
{
    public interface IForumsService
    {
        Task<Forum> GetById(int id);

        IEnumerable<Forum> GetAll();

        IEnumerable<ApplicationUser> GetAllActiveUsers();

        Task Create(Forum forum);

        Task Delete(int forumId);

        Task UpdateForumTitle(int forumId, string newTitle);

        Task UpdateForumDescription(int forumId, string newDescription);
    }
}
