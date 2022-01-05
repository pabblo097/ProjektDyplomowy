using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.DAL
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext() : base()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Report> Reports { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>(builder =>
            {
                builder.HasOne(u => u.User).WithMany(p => p.Posts).HasForeignKey(fk => fk.UserId).OnDelete(DeleteBehavior.SetNull);
                builder.HasMany(ul => ul.UsersWhoLikePost).WithMany(p => p.LikedPosts);
                builder.HasMany(t => t.Tags).WithOne(p => p.Post).HasForeignKey(fk => fk.PostId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Comment>(builder =>
            {
                builder.HasOne(u => u.User).WithMany(c => c.Comments).HasForeignKey(fk => fk.UserId).OnDelete(DeleteBehavior.SetNull);
            });




            //seed categories
            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.Parse("3a68ccb4-4829-4cae-986f-f6bdce215331"),
                Name = "Zwierzęta"
            });

            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.Parse("4210f47a-45f7-4cf6-afc7-bb5f217095f6"),
                Name = "Humor"
            });

            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.Parse("58e9b937-cd1e-4fa2-a72f-b17db44e848d"),
                Name = "Ciekawostki"
            });

            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.Parse("1bf0ab72-c36a-4fe7-b520-0b50be6495c9"),
                Name = "Polityka"
            });

            builder.Entity<Category>().HasData(new Category
            {
                Id = Guid.Parse("71cdcc66-6bc9-43c7-a87e-31c8848fce67"),
                Name = "Informatyka"
            });
        }
    }
}
