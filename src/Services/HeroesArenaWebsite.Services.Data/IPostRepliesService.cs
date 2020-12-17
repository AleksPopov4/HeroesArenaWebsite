using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models.Forum;

namespace HeroesArenaWebsite.Services.Data
{
    public interface IPostRepliesService
    {
        PostReply GetById(int id);

        Task Edit(int id, string newContent);

        Task Delete(int id);
    }
}
