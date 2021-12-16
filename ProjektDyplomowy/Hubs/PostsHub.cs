using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Repositories;

namespace ProjektDyplomowy.Hubs
{
    public class PostsHub : Hub
    {
        private readonly UserManager<User> userManager;
        private readonly IPostsRepository postsRepository;
        private readonly IMapper mapper;

        public PostsHub(UserManager<User> userManager, IPostsRepository postsRepository, IMapper mapper)
        {
            this.userManager = userManager;
            this.postsRepository = postsRepository;
            this.mapper = mapper;
        }

        public async Task LikePost(string postId)
        {
            string errorMessage = null;
            bool isSucceed = true;

            if (string.IsNullOrWhiteSpace(postId))
            {
                errorMessage = "postIdError";
                isSucceed = false;
            }

            var post = await postsRepository.GetPostByIdAsync(Guid.Parse(postId));

            if (post == null)
            {
                errorMessage = "postNotFound";
                isSucceed = false;
            }

            var userId = Context.UserIdentifier;
            var user = await userManager.FindByIdAsync(userId);

            if (isSucceed)
            {
                post.LikesQuantity++;
                post.UsersWhoLikePost.Add(user);
                if (!await postsRepository.UpdateAsync(post))
                {
                    errorMessage = "internalError";
                    isSucceed = false;
                }

            }

            await Clients.Caller.SendAsync("ReceiveLikePostStatus", isSucceed, errorMessage, post.LikesQuantity);
        }

        public async Task DislikePost(string postId)
        {
            string errorMessage = null;
            bool isSucceed = true;

            if (string.IsNullOrWhiteSpace(postId))
            {
                errorMessage = "postIdError";
                isSucceed = false;
            }

            var post = await postsRepository.GetPostByIdAsync(Guid.Parse(postId));

            if (post == null)
            {
                errorMessage = "postNotFound";
                isSucceed = false;
            }

            var userId = Context.UserIdentifier;
            var user = await userManager.FindByIdAsync(userId);

            if (isSucceed)
            {
                post.LikesQuantity--;
                var removeResult = post.UsersWhoLikePost.Remove(user);
                if (!await postsRepository.UpdateAsync(post) || !removeResult)
                {
                    errorMessage = "internalError";
                    isSucceed = false;
                }

            }

            await Clients.Caller.SendAsync("ReceiveDislikePostStatus", isSucceed, errorMessage, post.LikesQuantity);
        }
    }
}
