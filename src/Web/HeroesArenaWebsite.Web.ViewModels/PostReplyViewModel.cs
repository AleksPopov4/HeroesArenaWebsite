using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HeroesArenaWebsite.Web.ViewModels
{
    public class PostReplyViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorImageUrl { get; set; }

        public int AuthorRating { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ReplyContent { get; set; }

        public int PostId { get; set; }
    }
}
