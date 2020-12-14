using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HeroesArenaWebsite.Web.ViewModels.Forum
{
    public class ForumIndexViewModel
    {
        public IEnumerable<ForumListingViewModel> ForumList { get; set; }

        public int NumberOfForums { get; set; }
    }
}
