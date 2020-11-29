namespace HeroesArenaWebsite.Data.Models.Forum
{
    using HeroesArenaWebsite.Data.Common.Models;

    public class ForumPostReply : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ForumPost Post { get; set; }
    }
}
