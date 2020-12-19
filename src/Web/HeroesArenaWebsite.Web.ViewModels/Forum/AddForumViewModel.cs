using Microsoft.AspNetCore.Http;

namespace HeroesArenaWebsite.Web.ViewModels.Forum
{
    public class AddForumViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile ImageUpload { get; set; }
    }
}
