using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.Models.Posts
{
    public class PostsDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string FileName { get; set; }
        public SourceType SourceType { get; set; }
        public ContentType ContentType { get; set; }
        public int LikesQuantity { get; set; }
        public List<string> UsersWhoLikePost { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public string SortCommentsBy { get; set; }
        public string Username { get; set; }
        public Guid UserId { get; set; }
        public List<Tag> Tags { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
