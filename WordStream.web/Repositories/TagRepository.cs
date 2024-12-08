using Microsoft.EntityFrameworkCore;
using WordStream.web.Data;
using WordStream.web.Models.Domain;

namespace WordStream.web.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly WordStreamDbContext _wordStreamDbContext;

        public TagRepository(WordStreamDbContext wordStreamDbContext)
        {
           _wordStreamDbContext = wordStreamDbContext;
        }
        public async Task<Tag> AddAsync(Tag tag)
        {
            await _wordStreamDbContext.Tags.AddAsync(tag);
            await _wordStreamDbContext.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tags = await _wordStreamDbContext.Tags.FindAsync(id);

            if (tags != null)
            {
                _wordStreamDbContext.Tags.Remove(tags);
                await _wordStreamDbContext.SaveChangesAsync();

                return tags;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _wordStreamDbContext.Tags.ToListAsync();

        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _wordStreamDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async  Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _wordStreamDbContext.Tags.FindAsync(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                await _wordStreamDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }
    }
}
