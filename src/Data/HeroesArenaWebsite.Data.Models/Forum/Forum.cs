using System;
using System.Collections.Generic;
using System.Text;
using HeroesArenaWebsite.Data.Common.Models;

namespace HeroesArenaWebsite.Data.Models.Forum
{
    public class Forum : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<Post> Posts { get; set; }
    }
}
