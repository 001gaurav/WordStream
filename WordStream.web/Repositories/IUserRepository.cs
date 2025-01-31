using Microsoft.AspNetCore.Identity;

namespace WordStream.web.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
