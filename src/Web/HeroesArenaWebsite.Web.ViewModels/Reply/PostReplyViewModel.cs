using System;

namespace HeroesArenaWebsite.Web.ViewModels.Reply
{
    public class PostReplyViewModel
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorImageUrl { get; set; }

        public int AuthorRating { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ReplyContent { get; set; }

        public bool IsAuthorAdmin { get; set; }

        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }


        public int ForumId { get; set; }

        public string ForumTitle { get; set; }

        public string ForumImageUrl { get; set; }
    }
}
