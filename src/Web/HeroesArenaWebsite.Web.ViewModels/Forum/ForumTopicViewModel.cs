using System.Collections.Generic;
using HeroesArenaWebsite.Web.ViewModels.Post;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HeroesArenaWebsite.Web.ViewModels.Forum
{
    public class ForumTopicViewModel
    {
        public ForumListingViewModel Forum { get; set; }

        public IEnumerable<PostListingViewModel> Posts { get; set; }

        public string SearchQuery { get; set; }

        public bool EmptySearchResults { get; set; }
    }
}
