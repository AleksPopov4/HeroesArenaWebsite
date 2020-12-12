using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesArenaWebsite.Web.ViewModels.Post
{
    public class DeletePostViewModel
    {
        public int PostId { get; set; }

        public string PostAuthor { get; set; }

        public string PostContent { get; set; }
    }
}
