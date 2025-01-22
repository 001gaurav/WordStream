
using Microsoft.EntityFrameworkCore;
using WordStream.web.Data;
using WordStream.web.Models.Domain;

namespace WordStream.web.Repositories
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly WordStreamDbContext wordStreamDbContext;

        public BlogPostLikeRepository(WordStreamDbContext wordStreamDbContext)
        {
            this.wordStreamDbContext = wordStreamDbContext;
        }

        public async Task<BlogPostLike> AddLikeForBlog(BlogPostLike blogPostLike)
        {
            await wordStreamDbContext.BlogPostLike.AddAsync(blogPostLike);
            await wordStreamDbContext.SaveChangesAsync();
            return blogPostLike; 
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikeForBlog(Guid blogPostId)
        {
            return await wordStreamDbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await wordStreamDbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
        }
    }
}
