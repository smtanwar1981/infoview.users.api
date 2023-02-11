using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using Users.API.Controllers;
using Users.API.Custom;
using Users.API.Services;

namespace Users.API.Tests;

public class UsersControllerTest
{
    private readonly Mock<IUserService> _userService;

    public UsersControllerTest()
    {
        _userService = new Mock<IUserService>();
    }

    [Fact]
    public async void GetAllUsers_should_return_list_of_users()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var mockUsersList = GetUsersList();
        _userService.Setup(x => x.GetAllUsers(cancellationToken))
                .Returns(mockUsersList);
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.GetUsers(cancellationToken);
        var statusCodeResult = (IStatusCodeActionResult)result;

        // assert
        Assert.NotNull(result);
        Assert.Equal(200, statusCodeResult.StatusCode);
    }

    [Fact]
    public async void GetUserById_should_return_a_user()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var getUserByIdMockResponse = GetUserByIdResonse();
        _userService.Setup(x => x.GetUserById("123", cancellationToken))
                .Returns(Task.FromResult(getUserByIdMockResponse));
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.GetUserById("123", cancellationToken);
        var statusCodeResult = (IStatusCodeActionResult)result;

        // assert
        Assert.NotNull(result);
        Assert.Equal(200, statusCodeResult.StatusCode);
    }

    [Fact]
    public async void GetUserById_should_return_bad_request_status()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.GetUserById(null, cancellationToken);

        // assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void GetUserById_should_return_no_found_result()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        _userService.Setup(x => x.GetUserById("123", cancellationToken))
                .Returns(Task.FromResult((User)null));
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.GetUserById("123", cancellationToken);

        // assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void GetAllUsers_should_throw_an_exception_with_error_message()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var mockUsersList = GetEmptyUsersList();
        _userService.Setup(x => x.GetAllUsers(cancellationToken))
                .Returns(mockUsersList);
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.GetUsers(cancellationToken);
        var statusCodeResult = (IStatusCodeActionResult)result;

        // assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, statusCodeResult.StatusCode);
    }

    [Fact]
    public async void AddUser_should_return_bad_request_error()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.AddUser(null, cancellationToken);

        // assert
        Assert.IsType<BadRequestResult>(result);        
    }

    [Fact]
    public async void AddUser_should_return_an_exception_with_a_message()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        _userService.Setup(x => x.GetAllUsers(cancellationToken)).Returns(Task.FromResult((List<User>)null));
        var usersController = new UsersController(_userService.Object);
        var errorMessage = "Unable to add user.";

        var ex = await Assert.ThrowsAsync<CustomException>(() => usersController.AddUser(new User(), cancellationToken));

        // assert
        Assert.Equal(ex.Message, errorMessage);

    }

    [Fact]
    public async void AddUser_method_should_add_a_user()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var mockAddUserRequest = GetAddUserRequest();
        var mockAddUserResponse = GetAddUserResponse();
        _userService.Setup(x => x.AddUser(mockAddUserRequest, cancellationToken))
                .Returns(mockAddUserResponse);
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.AddUser(mockAddUserRequest, cancellationToken);
        var statusCodeResult = (IStatusCodeActionResult)result;

        // assert
        Assert.NotNull(result);
        Assert.Equal(200, statusCodeResult.StatusCode);
    }

    [Fact]
    public async void DeleteUser_should_return_bad_request_status()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.DeleteUser(null, cancellationToken);

        // assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void DeleteUser_should_throw_an_exception()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        _userService.Setup(x => x.DeleteUser("123", cancellationToken))
                .Returns(Task.FromResult((bool)false));
        var usersController = new UsersController(_userService.Object);
        var errorMessage = "Unable to delete this user.";

        // act
        var ex = await Assert.ThrowsAsync<CustomException>(() => usersController.DeleteUser("123", cancellationToken));

        // assert
        Assert.Equal(ex.Message, errorMessage);
    }

    [Fact]
    public async void DeleteUser_should_delete_the_user()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        _userService.Setup(x => x.DeleteUser("123", cancellationToken))
                .Returns(Task.FromResult((bool)true));
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.DeleteUser("123", cancellationToken);
        var statusCodeResult = (IStatusCodeActionResult)result;

        // assert
        Assert.Equal(200, statusCodeResult.StatusCode);
    }

    [Fact]
    public async void UpdateUser_should_return_bad_request_status()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.UpdateUser(null, cancellationToken);

        // assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void UpdateUser_should_return_bad_request_status_when_user_id_is_null()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.UpdateUser(new User { Id = Guid.Empty }, cancellationToken);

        // assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void UpdateUser_should_return_not_found_status_response()
    {
        //arrange
        CancellationTokenSource cts = new();
        CancellationToken cancellationToken = cts.Token;
        var guid = Guid.NewGuid();
        _userService.Setup(x => x.UpdateUser(new User { Id = guid }, cancellationToken))
                .Returns(Task.FromResult((bool)false));
        var usersController = new UsersController(_userService.Object);

        // act
        var result = await usersController.UpdateUser(new User { Id = guid }, cancellationToken);

        // assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    //[Fact]
    //public async void UpdateUser_should_update_the_user()
    //{
    //    //arrange
    //    CancellationTokenSource cts = new();
    //    CancellationToken cancellationToken = cts.Token;
    //    var guid = Guid.NewGuid();
    //    _userService.Setup(x => x.UpdateUser(GetUserByIdResonse(), cancellationToken))
    //            .Returns(Task.FromResult((bool)true));
    //    var usersController = new UsersController(_userService.Object);

    //    // act
    //    var result = await usersController.UpdateUser(GetUserByIdResonse(), cancellationToken);
    //    var statusCodeResult = (IStatusCodeActionResult)result;

    //    // assert
    //    Assert.Equal(200, statusCodeResult.StatusCode);
    //}

    private async Task<List<User>> GetUsersList()
    {
        return await Task.Run(() =>
        {
            return new List<User>
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
        });
    }

    private async Task<List<User>> GetEmptyUsersList()
    {
        return await Task.Run(() =>
        {
            return new List<User>();
        });
    }

    private User GetAddUserRequest()
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "testuser@test.com",
            IsActive = true
        };
    }

    private async Task<User> GetAddUserResponse()
    {
        return await Task.Run(() =>
        {
            return new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@test.com",
                IsActive = true
            };
        });
    }

    private User GetUserByIdResonse()
    {
        return new User
        {
            Email = "testuser@test.com",
            FirstName = "test",
            LastName = "user",
            Id = Guid.NewGuid(),
            IsActive = true,
        };
    }
}
