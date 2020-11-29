namespace HeroesArenaWebsite.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using HeroesArenaWebsite.Data.Common.Models;
    using HeroesArenaWebsite.Data.Models.Blog.BlogSystem.Data.Models;

    public class BlogPost : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        [DataType(DataType.Html)]
        public string Content { get; set; }

        [DataType(DataType.MultilineText)]
        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        [DataType(DataType.Html)]
        public string ShortContent { get; set; }

        public string ImageOrVideoUrl { get; set; }

        public BlogPostType Type { get; set; }

        public int? OldId { get; set; }
    }
}
