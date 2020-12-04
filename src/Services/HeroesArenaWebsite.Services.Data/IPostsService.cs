using System.Collections.Generic;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models.Forum;

namespace HeroesArenaWebsite.Services.Data
{
    public interface IPostsService
    {
        Post GetById(int id);

        IEnumerable<Post> GetAll();

        IEnumerable<Post> GetFilteredPosts(string searchQuery);

        IEnumerable<Post> GetPostsByForumId(int id);

        Task Add(Post post);

        Task Delete(Post post);

        Task EditPost(int id, string newContent);



        //Task<int> CreateAsync(string title, string content, int categoryId, string userId);

        //T GetById<T>(int id);

        //IEnumerable<T> GetByCategoryId<T>(int categoryId, int? take = null, int skip = 0);

        //int GetCountByCategoryId(int categoryId);
    }
}
