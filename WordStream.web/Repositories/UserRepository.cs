using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WordStream.web.Data;

namespace WordStream.web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;

        public UserRepository(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await authDbContext.Users.ToListAsync();

            var superAdminUser = await authDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == "superadmin@wordstream.com");

            if (superAdminUser is not null)
            {
                users.Remove(superAdminUser);
            }
            return users;
        }
    }
}
