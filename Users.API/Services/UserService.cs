using Users.API.Custom;
using Users.API.Repositories;

namespace Users.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        ///<summary>description</summary>
        public async Task<List<User>> GetAllUsers(CancellationToken token)
        {
            var users = await _userRepository.GetAllUsers(token).ConfigureAwait(false);
            if (users == null && users?.Count < 0)
            {
                return new List<User>();
            }
#pragma warning disable CS8603 // Possible null reference return.
            return users;
        }

        public async Task<User> GetUserById(string id, CancellationToken token)
        { 
            return await _userRepository.GetUserById(id, token).ConfigureAwait(false);
        }

        ///<summary>description</summary>
        public async Task<User> AddUser(User request, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrEmpty(request.FirstName))
            {
                throw new CustomException($"First name can not be empty.");
            }

            if (string.IsNullOrEmpty(request.LastName))
            {
                throw new CustomException($"Last name can not be empty.");
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                throw new CustomException($"Email can not be empty.");
            }

            var addedUser = new User();
            var users = await _userRepository.GetAllUsers(token).ConfigureAwait(false);
            if (users != null && users.Count > 0 && users.Any(x => x.Email.ToLowerInvariant() == request.Email.ToLowerInvariant()))
            {
                throw new CustomException($"Email already exist.");
            }

            try
            {
                request.Id = Guid.NewGuid();
                addedUser = await _userRepository.AddUser(request, token);
            }
            catch
            {
                throw;
            }
            return addedUser;
        }

        ///<summary>description</summary>
        public async Task<bool> UpdateUser(User request, CancellationToken token)
        {
            try
            {
                var existingUser = await _userRepository.GetUserById(request.Id.ToString(), token);
                if (existingUser is null)
                {
                    return false;
                }
                else
                {
                    existingUser.FirstName = string.IsNullOrEmpty(request.FirstName) ? existingUser.FirstName : request.FirstName;
                    existingUser.LastName = string.IsNullOrEmpty(request.LastName) ? existingUser.LastName : request.LastName;
                    existingUser.Email = string.IsNullOrEmpty(request.Email) ? existingUser.Email : request.Email;
                    existingUser.IsActive = request.IsActive ?? existingUser.IsActive;
                    return await _userRepository.UpdateUser(existingUser, token);
                }
            }
            catch
            {
                throw;
            }
        }

        ///<summary>description</summary>
        public async Task<bool> DeleteUser(string id, CancellationToken token)
        {
            try
            {
                var userToDelete = await _userRepository.GetUserById(id, token).ConfigureAwait(false);
                if (userToDelete != null)
                {
                    await _userRepository.DeleteUser(userToDelete, token);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
