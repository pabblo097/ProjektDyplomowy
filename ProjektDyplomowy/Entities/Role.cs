using Microsoft.AspNetCore.Identity;

namespace ProjektDyplomowy.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Role() : base()
        {
        }

        public Role(string name) : base(name)
        {
        }
    }
}
