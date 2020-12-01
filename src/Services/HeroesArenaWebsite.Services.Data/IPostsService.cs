using System.Collections.Generic;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models.Forum;

namespace HeroesArenaWebsite.Services.Data
{
    public interface IPostService
    {
        Task<Post> GetById(int id);

        IEnumerable<Post> GetAll();

        IEnumerable<Post> GetFilteredPosts(string searchQuery);

        Task Add(Post post);

        Task Delete(Post post);

        Task EditPost(int id, string newContent);
    }
}
