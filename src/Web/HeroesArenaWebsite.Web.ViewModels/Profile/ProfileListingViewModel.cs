using System.Collections.Generic;

namespace HeroesArenaWebsite.Web.ViewModels.Profile
{
    public class ProfileListingViewModel
    {
        public IEnumerable<ProfileViewModel> Profiles { get; set; }
    }
}
