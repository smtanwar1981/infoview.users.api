using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.API.Core;
using Users.API.Repositories;
using Users.API.Services;

namespace Users.API.Tests
{
    public  class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository;

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task GetAllUsers_should_return_users()
        {
            // arrange 
            var users = new List<User>
            {
                new User
                {
                    Email = "sandeep@test.com",
                    FirstName = "sandy",
                    LastName = "rocks",
                    IsActive = true,
                    Id = Guid.NewGuid(),
                },
                new User
                {
                    Email = "sandeep123@test.com",
                    FirstName = "sandy123",
                    LastName = "rocks123",
                    IsActive = true,
                    Id = Guid.NewGuid(),
                }
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetAllUsers(cancellationToken)).Returns(Task.FromResult(users));
            var userService = new UserService(_userRepository.Object);

            // Act
            var result = await userService.GetAllUsers(cancellationToken);

            // Assert
            Assert.Equal(users, result);
        }

        [Fact]
        public async Task GetAllUsers_should_return_empty_list()
        {
            // arrange 
            var users = new List<User>();
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetAllUsers(cancellationToken)).Returns(Task.FromResult(users));
            var userService = new UserService(_userRepository.Object);

            // Act
            var result = await userService.GetAllUsers(cancellationToken);

            // Assert
            Assert.Empty(users);
        }

        [Fact]
        public async Task GetUserById_should_return_a_user_by_Id()
        {
            // arrange
            var id = "a4664973-7f63-4276-a1ec-cc0fd39eb823";
            var user = new User
            {
                    Email = "test@test.com",
                    FirstName = "sandy",
                    LastName = "rocks",
                    IsActive = true,
                    Id = Guid.Parse(id)
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetUserById(id, cancellationToken)).Returns(Task.FromResult(user));
            var userService = new UserService(_userRepository.Object);

            // Act
            var result = await userService.GetUserById(id, cancellationToken);

            // Assert
            Assert.Equal(result.Id, Guid.Parse(id));
        }

        [Fact]
        public async Task AddUser_should_throw_argument_null_exception()
        {
            // arrange
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            var userService = new UserService(_userRepository.Object);
            var errorMessage = "Value cannot be null";

            // Act
            var result = await Assert.ThrowsAsync<ArgumentNullException>(() => userService.AddUser(null, cancellationToken));

            //Assert
            Assert.IsType<ArgumentNullException>(result);
            Assert.Contains(errorMessage, result.Message);
        }

        [Fact]
        public async Task AddUser_should_throw_an_exception_when_FirstName_is_null_or_empty()
        {
            // arrange
            var user = new User
            {
                Email = "test@test.com",
                FirstName = "",
                LastName = "rocks",
                IsActive = true,
                Id = Guid.NewGuid()
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            var userService = new UserService(_userRepository.Object);
            var errorMessage = "First name can not be empty.";

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => userService.AddUser(user, cancellationToken));

            //Assert
            Assert.IsType<Exception>(result);
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public async Task AddUser_should_throw_an_exception_when_LastName_is_null_or_empty()
        {
            // arrange
            var user = new User
            {
                Email = "test@test.com",
                FirstName = "test",
                LastName = "",
                IsActive = true,
                Id = Guid.NewGuid()
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            var userService = new UserService(_userRepository.Object);
            var errorMessage = "Last name can not be empty.";

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => userService.AddUser(user, cancellationToken));

            //Assert
            Assert.IsType<Exception>(result);
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public async Task AddUser_should_throw_an_exception_when_Email_is_null_or_empty()
        {
            // arrange
            var user = new User
            {
                Email = "",
                FirstName = "test",
                LastName = "user",
                IsActive = true,
                Id = Guid.NewGuid()
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            var userService = new UserService(_userRepository.Object);
            var errorMessage = "Email can not be empty.";

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => userService.AddUser(user, cancellationToken));

            //Assert
            Assert.IsType<Exception>(result);
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public async Task AddUser_should_throw_an_exception_when_Email_already_exist()
        {
            // arrange 
            var existingUsers = new List<User>
            {
                new User
                {
                    Email = "sandeep@test.com",
                    FirstName = "sandy",
                    LastName = "rocks",
                    IsActive = true,
                    Id = Guid.NewGuid(),
                },
                new User
                {
                    Email = "test@test.com",
                    FirstName = "sandy123",
                    LastName = "rocks123",
                    IsActive = true,
                    Id = Guid.NewGuid(),
                }
            };
            var newUser = new User
            {
                Email = "test@test.com",
                FirstName = "test",
                LastName = "user",
                IsActive = true,
                Id = Guid.NewGuid()
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetAllUsers(cancellationToken)).Returns(Task.FromResult(existingUsers));
            var userService = new UserService(_userRepository.Object);
            var errorMessage = "Email already exist.";

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => userService.AddUser(newUser, cancellationToken));

            //Assert
            Assert.IsType<Exception>(result);
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public async Task AddUser_should_add_new_user()
        {
            // arrange 
            var existingUsers = new List<User>
            {
                new User
                {
                    Email = "sandeep@test.com",
                    FirstName = "sandy",
                    LastName = "rocks",
                    IsActive = true,
                    Id = Guid.NewGuid(),
                },
                new User
                {
                    Email = "test@test.com",
                    FirstName = "sandy123",
                    LastName = "rocks123",
                    IsActive = true,
                    Id = Guid.NewGuid(),
                }
            };
            var newUser = new User
            {
                Email = "testing@testing.com",
                FirstName = "test",
                LastName = "user",
                IsActive = true
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetAllUsers(cancellationToken)).Returns(Task.FromResult(existingUsers));
            _userRepository.Setup(x => x.AddUser(newUser, cancellationToken)).Returns(Task.FromResult(newUser));
            var userService = new UserService(_userRepository.Object);

            // Act
            var result = await userService.AddUser(newUser, cancellationToken);

            // Assert
            Assert.Equal(newUser, result);
        }

        [Fact]
        public async Task UpdateUser_should_return_false_when_there_is_no_user_found_to_update()
        {
            // arrange
            var userToUpdate = new User
            {
                Email = "test@test.com",
                FirstName = "updated user name",
                LastName = "user",
                IsActive = true,
                Id = Guid.NewGuid()
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetUserById("123", cancellationToken)).Returns(Task.FromResult((User)null));
            var userService = new UserService(_userRepository.Object);

            // Act
            var result = await userService.UpdateUser(userToUpdate, cancellationToken);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateUser_should_throw_an_exception_when_email_already_exist()
        {
            // arrange
            var userToUpdateUniqueId = "a4664973-7f63-4276-a1ec-cc0fd39eb823";
            var existingUsers = new List<User>
            {
                new User
                {
                    Email = "sandeep@test.com",
                    FirstName = "sandy",
                    LastName = "rocks",
                    IsActive = true,
                    Id = Guid.NewGuid(),
                },
                new User
                {
                    Email = "test@test.com",
                    FirstName = "sandy123",
                    LastName = "rocks123",
                    IsActive = true,
                    Id = Guid.Parse(userToUpdateUniqueId),
                }
            };
            var existingUser = new User
            {
                Email = "test@test.com",
                FirstName = "test",
                LastName = "user",
                IsActive = true,
                Id = Guid.Parse(userToUpdateUniqueId)
            };
            var userToUpdate = new User
            {
                Email = "test@test.com",
                FirstName = "updated user name",
                LastName = "user",
                IsActive = true,
                Id = Guid.Parse(userToUpdateUniqueId)
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetUserById(userToUpdateUniqueId, cancellationToken)).Returns(Task.FromResult(existingUser));
            _userRepository.Setup(x => x.GetAllUsers(cancellationToken)).Returns(Task.FromResult(existingUsers));
            var userService = new UserService(_userRepository.Object);
            var errorMessage = "Email already exist.";

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => userService.AddUser(userToUpdate, cancellationToken));

            //Assert
            Assert.IsType<Exception>(result);
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public async Task UpdateUser_should_update_user()
        {
            // arrange
            var existingUserUniqueId = "a4664973-7f63-4276-a1ec-cc0fd39eb823";
            var existingUser = new User
            {
                Email = "test@test.com",
                FirstName = "sandy123",
                LastName = "rocks123",
                IsActive = true,
                Id = Guid.Parse(existingUserUniqueId)
            };

            var existingUsers = new List<User>
            {
                new User
                {
                    Email = "sandeep@test.com",
                    FirstName = "sandy",
                    LastName = "rocks",
                    IsActive = true,
                    Id = Guid.NewGuid(),
                },
                new User
                {
                    Email = "test@test.com",
                    FirstName = "sandy123",
                    LastName = "rocks123",
                    IsActive = true,
                    Id = Guid.Parse(existingUserUniqueId),
                }
            };
            var userToUpdate = new User
            {
                Email = "test@test.com",
                FirstName = "test",
                LastName = "user",
                IsActive = false,
                Id = Guid.Parse(existingUserUniqueId)
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetUserById(existingUserUniqueId, cancellationToken)).Returns(Task.FromResult(existingUser));
            _userRepository.Setup(x => x.GetAllUsers(cancellationToken)).Returns(Task.FromResult(existingUsers));
            _userRepository.Setup(x => x.UpdateUser(userToUpdate, cancellationToken)).Returns(Task.FromResult(true));
            var userService = new UserService(_userRepository.Object);

            // Act
            var result = await userService.UpdateUser(userToUpdate, cancellationToken);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUser_should_throw_an_exception_when_user_not_found()
        {
            // Arrange
            var existingUserUniqueId = "a4664973-7f63-4276-a1ec-cc0fd39eb823";
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetUserById(existingUserUniqueId, cancellationToken)).Returns(Task.FromResult((User)null));
            var userService = new UserService(_userRepository.Object); 
            var errorMessage = "User not found.";

            // Act
            var result = await Assert.ThrowsAsync<Exception>(() => userService.DeleteUser(existingUserUniqueId, cancellationToken));

            //Assert
            Assert.IsType<Exception>(result);
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public async Task DeleteUser_should_delete_user()
        {
            // Arrange
            var existingUserUniqueId = "a4664973-7f63-4276-a1ec-cc0fd39eb823";
            var existingUser = new User
            {
                Email = "test@test.com",
                FirstName = "test",
                LastName = "user",
                IsActive = true,
                Id = Guid.Parse(existingUserUniqueId)
            };
            CancellationTokenSource cts = new();
            CancellationToken cancellationToken = cts.Token;
            _userRepository.Setup(x => x.GetUserById(existingUserUniqueId, cancellationToken)).Returns(Task.FromResult(existingUser));
            var userService = new UserService(_userRepository.Object);

            // Act
            var result = await userService.DeleteUser(existingUserUniqueId, cancellationToken);

            //Assert
            Assert.True(result);
        }
    }
}
