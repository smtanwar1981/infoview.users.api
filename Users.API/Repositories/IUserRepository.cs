namespace Users.API.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Fetch the list of existing Users.
        /// </summary>
        /// <param name="token">An api cancellation token.</param>
        /// <returns></returns>
        public Task<List<User>> GetAllUsers(CancellationToken token);

        /// <summary>
        /// Fetch list of existing users from the database.
        /// </summary>
        /// <param name="token">An api cancellation token.</param>
        /// <returns>List of existing users.</returns>
        public Task<User> AddUser(User request, CancellationToken token);

        /// <summary>
        /// Get user details by Id.
        /// </summary>
        /// <param name="Id">A unique identifier.</param>
        /// <returns>User details.</returns>
        Task<User> GetUserById(string Id, CancellationToken token);

        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <param name="request">User model.</param>
        /// <param name="token">An api cancellation token.</param>
        /// <returns>A boolean value true if user deleted and false if not.</returns>
        public Task<bool> DeleteUser(User request, CancellationToken token);

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="request">User model.</param>
        /// <param name="token">Api cancellation token.</param>
        /// <returns>True if user successfully update, false if not.</returns>
        public Task<bool> UpdateUser(User request, CancellationToken token);
    }
}
