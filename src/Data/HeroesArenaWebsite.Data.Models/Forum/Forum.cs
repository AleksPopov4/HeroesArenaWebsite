namespace HeroesArenaWebsite.Data.Models.Forum
{
    using System;
    using System.Collections.Generic;

    using HeroesArenaWebsite.Data.Common.Models;

    public class Forum : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        //public DateTime Created { get; set; } 

        public string ImageUrl { get; set; }

        public virtual IEnumerable<ForumPost> Posts{ get; set; }
    }
}
