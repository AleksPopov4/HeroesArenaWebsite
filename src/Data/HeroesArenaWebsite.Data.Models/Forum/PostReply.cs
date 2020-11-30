using System;
using System.Collections.Generic;
using System.Text;
using HeroesArenaWebsite.Data.Common.Models;

namespace HeroesArenaWebsite.Data.Models.Forum
{
    public class PostReply : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Post Post { get; set; }
    }
}
