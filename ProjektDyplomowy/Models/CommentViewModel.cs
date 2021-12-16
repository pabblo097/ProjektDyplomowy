namespace ProjektDyplomowy.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public int LikesQuantity { get; set; }
        public List<string> UsersWhoLikeComment { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
