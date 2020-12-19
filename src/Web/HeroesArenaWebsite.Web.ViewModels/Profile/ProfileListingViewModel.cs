using System.Collections.Generic;
using HeroesArenaWebsite.Web.ViewModels.Forum;

namespace HeroesArenaWebsite.Web.ViewModels.Profile
{
    public class ProfileListingViewModel
    {
        public IEnumerable<ProfileViewModel> Profiles { get; set; }
    }
}
