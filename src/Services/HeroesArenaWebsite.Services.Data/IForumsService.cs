using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Data.Models.Forum;

namespace HeroesArenaWebsite.Services.Data
{
    public interface IForumsService
    {
        Forum GetById(int id);

        IEnumerable<Forum> GetAll();

        IEnumerable<ApplicationUser> GetActiveUsers(int forumId);

        IEnumerable<Post> GetFilteredPosts(string searchQuery);

        IEnumerable<Post> GetFilteredPosts(int forumId, string searchQuery);

        Post GetLatestPost(int forumId);

        bool HasRecentPost(int id);

        Task Add(Forum forum);

        //Task Create(Forum forum);

        Task DeleteAsync(int id);

        Task SetForumImage(int id, Uri uri);

        Task UpdateForumTitle(int id, string newTitle);

        Task UpdateForumDescription(int id, string newDescription);
    }
}
