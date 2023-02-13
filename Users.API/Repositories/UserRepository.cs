using Microsoft.EntityFrameworkCore;
using Users.API.Core;

namespace Users.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _dbContext;
        public UserRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        ///<summary>description</summary>
        public async Task<List<User>> GetAllUsers(CancellationToken token)
        {
            return await _dbContext.Users.ToListAsync(token).ConfigureAwait(false);
        }

        ///<summary>description</summary>
        public async Task<User> AddUser(User request, CancellationToken token)
        {
            try
            {
                _dbContext.Users.Add(request);
                await _dbContext.SaveChangesAsync(token);
            }
            catch
            {
                throw;
            }
            return request;
        }

        ///<summary>description</summary>
        public async Task<User> GetUserById(string Id, CancellationToken token)
        {
            var user = new User();
            try
            {
                user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == new Guid(Id));
            }
            catch
            {
                throw;
            }
            return user;
        }

        ///<summary>description</summary>
        public async Task<bool> DeleteUser(User request, CancellationToken token)
        {
            var deleteResult = false;
            try
            {
                _dbContext.Users.Remove(request);
                await _dbContext.SaveChangesAsync(token);
                deleteResult = true;
            }
            catch
            {
                throw;
            }
            return deleteResult;
        }

        ///<summary>description</summary>
        public async Task<bool> UpdateUser(User request, CancellationToken token)
        {
            var updateResult = false;
            try
            {
                _dbContext.Users.Update(request);
                await _dbContext.SaveChangesAsync(token);
                updateResult = true;
            }
            catch
            {
                throw;
            }
            return updateResult;
        }
    }
}
