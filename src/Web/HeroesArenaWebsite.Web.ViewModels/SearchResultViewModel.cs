using System.Collections.Generic;
using HeroesArenaWebsite.Web.ViewModels.Post;

namespace HeroesArenaWebsite.Web.ViewModels
{
    public class SearchResultViewModel
    {
        public IEnumerable<PostListingViewModel> Posts { get; set; }

        public string SearchQuery { get; set; }

        public bool EmptySearchResults { get; set; }
    }
}
