using Microsoft.EntityFrameworkCore;
using WordStream.web.Models.Domain;

namespace WordStream.web.Data
{
    public class WordStreamDbContext : DbContext
    {
        public WordStreamDbContext(DbContextOptions<WordStreamDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> BlogPostLike { get; set; }

        public DbSet<BlogPostComment> BlogPostComment { get; set; }
    }
}
