using System.Linq;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Common.Repositories;
using HeroesArenaWebsite.Data.Models.Forum;
using Microsoft.EntityFrameworkCore;

namespace HeroesArenaWebsite.Services.Data
{
    public class PostRepliesService : IPostRepliesService
    {
        private readonly IDeletableEntityRepository<PostReply> postRepliesRepository;

        public PostRepliesService(IDeletableEntityRepository<PostReply> postRepliesRepository)
        {
            this.postRepliesRepository = postRepliesRepository;
        }

        public PostReply GetById(int id)
        {
            return this.postRepliesRepository
                .All()
                .Include(reply => reply.Post)
                    .ThenInclude(post => post.Forum)
                .Include(reply => reply.Post)
                .ThenInclude(post => post.User)
                .FirstOrDefault(reply => reply.Id == id);
        }

        public async Task Edit(int id, string newContent)
        {
            var reply = this.GetById(id);
            reply.Content = newContent;
            this.postRepliesRepository.Update(reply);

            await this.postRepliesRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var reply = this.GetById(id);
            this.postRepliesRepository.Delete(reply);

            await this.postRepliesRepository.SaveChangesAsync();
        }
    }
}
