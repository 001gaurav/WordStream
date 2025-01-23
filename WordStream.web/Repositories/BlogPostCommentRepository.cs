using Microsoft.EntityFrameworkCore;
using WordStream.web.Data;
using WordStream.web.Models.Domain;

namespace WordStream.web.Repositories
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly WordStreamDbContext wordStreamDbContext;

        public BlogPostCommentRepository(WordStreamDbContext wordStreamDbContext)
        {
            this.wordStreamDbContext = wordStreamDbContext;
        }
        public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
        {
            await wordStreamDbContext.BlogPostComment.AddAsync(blogPostComment);
            await wordStreamDbContext.SaveChangesAsync();
            return blogPostComment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByBlogIdAsync(Guid blogPostId)
        {
            return await wordStreamDbContext.BlogPostComment.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
