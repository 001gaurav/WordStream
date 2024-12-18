using Microsoft.EntityFrameworkCore;
using WordStream.web.Data;
using WordStream.web.Models.Domain;

namespace WordStream.web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly WordStreamDbContext wordStreamDbContext;

        public BlogPostRepository(WordStreamDbContext wordStreamDbContext)
        {
            this.wordStreamDbContext = wordStreamDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await wordStreamDbContext.AddAsync(blogPost);
            await wordStreamDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
           var deleteBlog = await wordStreamDbContext.BlogPosts.FindAsync(id);
            if (deleteBlog != null)
            {
                wordStreamDbContext.BlogPosts.Remove(deleteBlog);
                await wordStreamDbContext.SaveChangesAsync();

                return deleteBlog;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await wordStreamDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await wordStreamDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await wordStreamDbContext.BlogPosts.Include(x => x.Tags)
                 .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Author = blogPost.Author;
                existingBlog.visible = blogPost.visible;
                existingBlog.Tags = blogPost.Tags;

                await wordStreamDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }
    }
}
