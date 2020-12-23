using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HeroesArenaWebsite.Web.ViewModels.Forum
{
    public class AddForumInputModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile ImageUpload { get; set; }
    }
}
