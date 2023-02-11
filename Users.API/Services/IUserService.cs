namespace Users.API.Services
{
    public interface IUserService
    {
        /// <summary>
        /// A service method to fetch users list.
        /// </summary>
        /// <param name="token">Api cancellation token.</param>
        /// <returns>List of users.</returns>
        public Task<List<User>> GetAllUsers(CancellationToken token);

        /// <summary>
        /// A service method to fetch user by unique Id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <param name="token">Api cancellation token.</param>
        /// <returns>User model.</returns>
        public Task<User> GetUserById(string id, CancellationToken token);

        /// <summary>
        /// A service method to add new user to the system.
        /// </summary>
        /// <param name="request">A user to be added.</param>
        /// <param name="token">Api cancellation token.</param>
        /// <returns>Added user.</returns>
        public Task<User> AddUser(User request, CancellationToken token);

        /// <summary>
        /// A service method to delete an existing user.
        /// </summary>
        /// <param name="id">To unique identify the user and delete it.</param>
        /// <param name="token">Api cancellation token.</param>
        /// <returns></returns>
        public Task<bool> DeleteUser(string id, CancellationToken token);

        /// <summary>
        /// A service method to update a user.
        /// </summary>
        /// <param name="request">User to udpate.</param>
        /// <param name="token">Api cancellation token.</param>
        /// <returns>returns True if user is successfully update else False.</returns>
        public Task<bool> UpdateUser(User request, CancellationToken token);
    }
}
