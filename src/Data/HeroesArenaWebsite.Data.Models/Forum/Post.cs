using System;
using System.Collections.Generic;
using HeroesArenaWebsite.Data.Common.Models;

namespace HeroesArenaWebsite.Data.Models.Forum
{
    public class Post : BaseDeletableModel<int>
    {
        public Post()
        {
            this.Replies = new HashSet<PostReply>();
        }

        public virtual ApplicationUser User { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsArchived { get; set; }

        public virtual IEnumerable<PostReply> Replies { get; set; }

        public virtual Forum Forum { get; set; }
    }
}
