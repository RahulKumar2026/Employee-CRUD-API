using Employee_CRUD_API.Data;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Employee_CRUD_API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbcontext;
        private ILogger<UserRepository> _logger;
        public UserRepository(AppDbContext appDbContext, ILogger<UserRepository> logger) 
        {
            _logger = logger;
            _dbcontext = appDbContext;
        }
        public async Task<User?> GetByUsernameAsync(string username) 
        {
            try
            {
                var result = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsActive);
                return result; 
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while accessing DataBase StatusCode: {StatusCode}", HTTPResponseWrapper.Constants.BadRequestCode);
                throw;
            }
        }
    }
}
