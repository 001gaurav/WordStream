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
        public async Task<IEnumerable<Tag>> GetAllAsync(
            string? searchQuery, 
            string? sortBy,
            string? sortDirection,
            int pageSize = 1,
            int pageNumber = 100)
        {
            var query = _wordStreamDbContext.Tags.AsQueryable();

            //Filtering
            if (string.IsNullOrWhiteSpace(searchQuery) == false)
            {
                query = query.Where(x => x.Name.Contains(searchQuery) ||
                                         x.DisplayName.Contains(searchQuery));
            }

            // Sorting 
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }
                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName);
                }

            }

            // Pagination
            var skipRecords = (pageNumber - 1) * pageSize;
            query = query.Skip(skipRecords).Take(pageSize);


            return await query.ToListAsync();

            // return await _wordStreamDbContext.Tags.ToListAsync();

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
        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _wordStreamDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Tag?> UpdateAsync(Tag tag)
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

        public async Task<int> CountAsync()
        {
            return await _wordStreamDbContext.Tags.CountAsync();
        }
    }
}
