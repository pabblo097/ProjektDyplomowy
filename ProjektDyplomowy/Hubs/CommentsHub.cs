using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ProjektDyplomowy.Entities;
using ProjektDyplomowy.Models;
using ProjektDyplomowy.Repositories;

namespace ProjektDyplomowy.Hubs
{
    [Authorize]
    public class CommentsHub : Hub
    {
        private readonly UserManager<User> userManager;
        private readonly ICommentsRepository commentsRepository;
        private readonly IMapper mapper;

        public CommentsHub(UserManager<User> userManager, ICommentsRepository commentsRepository, IMapper mapper)
        {
            this.userManager = userManager;
            this.commentsRepository = commentsRepository;
            this.mapper = mapper;
        }

        public async Task AddComment(string comment, string postId)
        {
            string errorMessage = null;
            bool isSucceed = true;

            if (string.IsNullOrWhiteSpace(comment))
            {
                errorMessage = "Komentarz nie może być pusty";
                isSucceed = false;
            }

            if (string.IsNullOrWhiteSpace(postId))
            {
                errorMessage = "postIdError";
                isSucceed = false;
            }


            var userId = Context.UserIdentifier;
            var user = await userManager.FindByIdAsync(userId);

            var newComment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = comment,
                CreationDate = DateTime.Now,
                LikesQuantity = 0,
                PostId = Guid.Parse(postId),
                UserId = Guid.Parse(userId),
                User = user
            };

            if (isSucceed)
            {
                if (!await commentsRepository.AddAsync(newComment))
                {
                    errorMessage = "internalError";
                    isSucceed = false;
                }

            }

            var newCommentViewModel = mapper.Map<CommentViewModel>(newComment);

            await Clients.Caller.SendAsync("ReceiveComment", isSucceed, errorMessage, newCommentViewModel);
        }

        public async Task DeleteComment(string commentId)
        {
            string errorMessage = null;
            bool isSucceed = true;

            if (string.IsNullOrWhiteSpace(commentId))
            {
                errorMessage = "commentIdError";
                isSucceed = false;
            }

            var comment = await commentsRepository.GetCommentByIdAsync(Guid.Parse(commentId));

            if (comment == null)
            {
                errorMessage = "commentNotFound";
                isSucceed = false;
            }

            var userId = Context.UserIdentifier;
            var user = await userManager.FindByIdAsync(userId);

            if (Guid.Parse(userId) != comment.UserId && !await userManager.IsInRoleAsync(user, "Admin"))
            {
                errorMessage = "Nie jesteś uprawniony do modyfikacji tego komentarza";
                isSucceed = false;
            }


            if (isSucceed)
            {
                if (!await commentsRepository.RemoveAsync(comment))
                {
                    errorMessage = "internalError";
                    isSucceed = false;
                }

            }

            await Clients.Caller.SendAsync("ReceiveDeleteStatus", isSucceed, errorMessage);
        }

        public async Task EditComment(string commentId, string newComment)
        {
            string errorMessage = null;
            bool isSucceed = true;

            if (string.IsNullOrWhiteSpace(commentId))
            {
                errorMessage = "commentIdError";
                isSucceed = false;
            }

            if (string.IsNullOrWhiteSpace(newComment))
            {
                errorMessage = "Komentarz nie może być pusty";
                isSucceed = false;
            }

            var comment = await commentsRepository.GetCommentByIdAsync(Guid.Parse(commentId));

            if (comment == null)
            {
                errorMessage = "commentNotFound";
                isSucceed = false;
            }

            var userId = Context.UserIdentifier;
            var user = await userManager.FindByIdAsync(userId);

            if (Guid.Parse(userId) != comment.UserId && !await userManager.IsInRoleAsync(user, "Admin"))
            {
                errorMessage = "Nie jesteś uprawniony do modyfikacji tego komentarza";
                isSucceed = false;
            }


            if (isSucceed)
            {
                comment.Content = newComment;
                if (!await commentsRepository.UpdateAsync(comment))
                {
                    errorMessage = "internalError";
                    isSucceed = false;
                }

            }

            await Clients.Caller.SendAsync("ReceiveEditStatus", isSucceed, errorMessage, newComment);
        }

        public async Task LikeComment(string commentId)
        {
            string errorMessage = null;
            bool isSucceed = true;

            if (string.IsNullOrWhiteSpace(commentId))
            {
                errorMessage = "commentIdError";
                isSucceed = false;
            }

            var comment = await commentsRepository.GetCommentByIdAsync(Guid.Parse(commentId));

            if (comment == null)
            {
                errorMessage = "commentNotFound";
                isSucceed = false;
            }

            var userId = Context.UserIdentifier;
            var user = await userManager.FindByIdAsync(userId);

            if (isSucceed)
            {
                comment.LikesQuantity++;
                comment.UsersWhoLikeComment.Add(user);
                if (!await commentsRepository.UpdateAsync(comment))
                {
                    errorMessage = "internalError";
                    isSucceed = false;
                }

            }

            await Clients.Caller.SendAsync("ReceiveLikeStatus", isSucceed, errorMessage, comment.LikesQuantity);
        }

        public async Task DislikeComment(string commentId)
        {
            string errorMessage = null;
            bool isSucceed = true;

            if (string.IsNullOrWhiteSpace(commentId))
            {
                errorMessage = "commentIdError";
                isSucceed = false;
            }

            var comment = await commentsRepository.GetCommentByIdAsync(Guid.Parse(commentId));

            if (comment == null)
            {
                errorMessage = "commentNotFound";
                isSucceed = false;
            }

            var userId = Context.UserIdentifier;
            var user = await userManager.FindByIdAsync(userId);

            if (isSucceed)
            {
                comment.LikesQuantity--;
                var removeResult = comment.UsersWhoLikeComment.Remove(user);
                if (!await commentsRepository.UpdateAsync(comment) || !removeResult)
                {
                    errorMessage = "internalError";
                    isSucceed = false;
                }

            }

            await Clients.Caller.SendAsync("ReceiveDislikeStatus", isSucceed, errorMessage, comment.LikesQuantity);
        }
    }
}
