﻿using System;
using System.Threading.Tasks;
using HeroesArenaWebsite.Common;
using HeroesArenaWebsite.Data.Models;
using HeroesArenaWebsite.Data.Models.Forum;
using HeroesArenaWebsite.Services.Data;
using HeroesArenaWebsite.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeroesArenaWebsite.Web.Controllers
{
    public class PostRepliesController : Controller
    {
        private readonly IForumsService forumsService;
        private readonly IPostsService postsService;
        private readonly ApplicationUser userService;

        private readonly UserManager<ApplicationUser> usersManager;

        public PostRepliesController(IForumsService forumsService, IPostsService postsService, ApplicationUser userService, UserManager<ApplicationUser> usersManager)
        {
            this.forumsService = forumsService;
            this.postsService = postsService;
            this.userService = userService;
            this.usersManager = usersManager;
        }

        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var post = this.postsService.GetById(id);
            var forum = this.forumsService.GetById(post.Forum.Id);
            var user = await this.usersManager.FindByNameAsync(this.User.Identity.Name);

            var model = new PostReplyViewModel
            {
                AuthorId = user.Id,
                AuthorName = this.User.Identity.Name,
                AuthorImageUrl = user.ProfileImageUrl,
                AuthorRating = user.Rating,
                CreatedOn = user.CreatedOn,

                PostId = post.Id,
                PostTitle = post.Title,
                PostContent = post.Content,

                ForumId = forum.Id,
                ForumTitle = forum.Title,
                ForumImageUrl = forum.ImageUrl,
                //IsAuthorAdmin = user.    //.Roles.Contains("admin"),
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReply(PostReplyViewModel model)
        {
            var userId = this.usersManager.GetUserId(User);
            var user = await this.usersManager.FindByIdAsync(userId);

            var post = this.postsService.GetById(model.Id);
            var reply = new PostReply
            {
                Post = post,
                Content = model.ReplyContent,
                User = user,
                CreatedOn = DateTime.UtcNow,
            };

            //await this.postsService;
        }
    }
}
