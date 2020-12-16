using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models;

namespace HeroesArenaWebsite.Services.Data
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);

        ApplicationUser GetByName(string name);

        IEnumerable<ApplicationUser> GetAll();

        Task IncrementRating(string id);

        Task Add(ApplicationUser user);

        Task Deactivate(ApplicationUser user);

        Task SetProfileImage(string id, Uri uri);

        Task BumpRating(string userId, Type type);
    }
}
