using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.API.Custom;
using Users.API.Services;

namespace Users.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("getUsers")]
    public async Task<IActionResult> GetUsers(CancellationToken token)
    {
        var results = await _userService.GetAllUsers(token).ConfigureAwait(false);
        if (results is null || results.Count == 0)
        {
            return NotFound("No user found.");
        }
        return Ok(results);
    }

    [HttpGet("getUserById/{id}")]
    public async Task<IActionResult> GetUserById(string id, CancellationToken token)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("User Id can not be null or empty.");
        }
        var result = await _userService.GetUserById(id, token).ConfigureAwait(false);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost("addNewUser")]
    public async Task<IActionResult> AddUser(User request, CancellationToken token)
    {
        if (request is null)
        {
            return BadRequest();
        }

        var result = await _userService.AddUser(request, token).ConfigureAwait(false);
        if (result is null)
        {
            throw new CustomException("Unable to add user.");
        }
        return Ok(result);
    }

    [HttpDelete("deleteUser")]
    public async Task<IActionResult> DeleteUser(string Id, CancellationToken token)
    {
        if (string.IsNullOrEmpty(Id))
        {
            return BadRequest("Id can not be null or empty.");
        }

        var result = await _userService.DeleteUser(Id, token).ConfigureAwait(false);
        if (!result)
        {
            throw new CustomException("Unable to delete this user.");
        }
        return Ok(result);
    }

    [HttpPut("updateUser")]
    public async Task<IActionResult> UpdateUser(User request, CancellationToken token)
    {
        if (request is null)
        {
            return BadRequest("Bad request");
        }
        if (string.IsNullOrEmpty(request.Id.ToString()) || request.Id == Guid.Empty)
        {
            return BadRequest("User id can not be empty or null.");
        }

        var result = await _userService.UpdateUser(request, token).ConfigureAwait(false);
        if (!result)
        {
            return NotFound("User either not found or unable to update.");
        }

        return Ok(result);
    }
}
