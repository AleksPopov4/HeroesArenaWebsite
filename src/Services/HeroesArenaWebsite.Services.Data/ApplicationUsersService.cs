using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Common.Repositories;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Data.Models.Forum;
using HeroesArenaWebsite.Services.Data;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class ApplicationUsersService : IApplicationUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public ApplicationUser GetById(string id)
        {
            return this.usersRepository
                .All()
                .FirstOrDefault(user => user.Id == id);
        }

        public ApplicationUser GetByName(string name)
        {
            return this.usersRepository
                .All()
                .FirstOrDefault(user => user.UserName == name);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return this.usersRepository.All();
        }

        public async Task IncrementRating(string id)
        {
            var user = this.GetById(id);
            user.Rating++;

            await this.usersRepository.SaveChangesAsync();

        }

        public async Task Add(ApplicationUser user)
        {
            await this.usersRepository.AddAsync(user);
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task Deactivate(ApplicationUser user)
        {
            user.IsActive = false;
            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = this.GetById(id);
            user.Rating++;
            this.usersRepository.Update(user);

            await this.usersRepository.SaveChangesAsync();
        }

        public async Task BumpRating(string userId, Type type)
        {
            var user = this.GetById(userId);
            var increment = GetIncrement(type);
            user.Rating += increment;

            await this.usersRepository.SaveChangesAsync();
        }

        private static int GetIncrement(Type type)
        {
            var bump = 0;

            if (type == typeof(Post))
            {
                bump = 3;
            }

            if (type == typeof(PostReply))
            {
                bump = 2;
            }

            return bump;
        }
    }
}
