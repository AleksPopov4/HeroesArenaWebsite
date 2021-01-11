using System;

namespace HeroesArenaWebsite.Web.ViewModels.Post
{
    public class DeletePostViewModel
    {
        public int ForumId { get; set; }

        public int PostId { get; set; }

        public string PostAuthor { get; set; }

        public string PostContent { get; set; }

        public string AuthorName { get; set; }

        public string AuthorId { get; set; }

        public DateTime CreatedOn { get; set; }

        public int AuthorRating { get; set; }

        public bool IsAuthorAdmin { get; set; }

        public string AuthorImageUrl { get; set; }
    }
}
