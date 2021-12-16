using Microsoft.AspNetCore.Identity;

namespace ProjektDyplomowy.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User() : base()
        {

        }

        public User(string userName) : base(userName)
        {

        }

        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Post> LikedPosts { get; set; }
        public List<Comment> LikedComments { get; set; }
    }
}
