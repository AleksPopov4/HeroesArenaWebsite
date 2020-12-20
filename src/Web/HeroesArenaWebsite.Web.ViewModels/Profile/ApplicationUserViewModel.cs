namespace HeroesArenaWebsite.Web.ViewModels.Profile
{
    public class ApplicationUserViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string ProfileImageUrl { get; set; }

        public int Rating { get; set; }

        public bool IsActive { get; set; }
    }
}
