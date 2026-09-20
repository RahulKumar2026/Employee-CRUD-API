using Employee_CRUD_API.Models;

namespace Employee_CRUD_API.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
