﻿using System.Collections.Generic;
using System.Threading.Tasks;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Data.Models.Forum;

namespace HeroesArenaWebsite.Services.Data
{
    public interface IPostsService
    {
        Post GetById(int id);

        IEnumerable<Post> GetAll();

        IEnumerable<Post> GetPostsByUserId(int id);

        IEnumerable<Post> GetFilteredPosts(string searchQuery);

        IEnumerable<Post> GetPostsByForumId(int id);

        IEnumerable<ApplicationUser> GetAllUsers(IEnumerable<Post> posts);

        Task Add(Post post);

        Task AddReply(PostReply reply);

        Task Archive(int id);

        Task DeleteAsync(int id);

        Task EditPost(int id, string newTitle, string newContent);

        string GetForumImageUrl(int id);
    }
}
