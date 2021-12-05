namespace ProjektDyplomowy.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Content { get; set; }
        public ContentType ContentType { get; set; }
        public int LikesQuantity { get; set; }
        public List<Comment> Comments { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public List<Tag> Tags { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
