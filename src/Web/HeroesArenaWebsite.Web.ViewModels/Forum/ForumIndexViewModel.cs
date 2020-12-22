using System.Collections.Generic;

namespace HeroesArenaWebsite.Web.ViewModels.Forum
{
    public class ForumIndexViewModel
    {
        public string SearchQuery { get; set; }

        public IEnumerable<ForumListingViewModel> ForumList { get; set; }

        public int NumberOfForums { get; set; }
    }
}
