using System;
using System.ComponentModel.DataAnnotations;

namespace HeroesArenaWebsite.Web.ViewModels.Post
{
    public class CreatePostInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string ForumName { get; set; }

        public string ForumImageUrl { get; set; }

        public int ForumId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }

        public string AuthorName { get; set; }
    }
}
