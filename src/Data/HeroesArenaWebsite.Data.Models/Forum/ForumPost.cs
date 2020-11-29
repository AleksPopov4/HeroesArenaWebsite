namespace HeroesArenaWebsite.Data.Models.Forum
{
    using System.Collections.Generic;

    using HeroesArenaWebsite.Data.Common.Models;

    public class ForumPost : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Forum Forum { get; set; }

        public virtual IEnumerable<ForumPostReply> Replies{ get; set; }
    }
}
