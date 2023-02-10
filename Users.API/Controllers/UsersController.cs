using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Users.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(UserContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var results = await _context.Users.Where(x => x.IsActive).ToListAsync();
        return Ok(results);
    }

    // GET: api/Users/...
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var result = await _context.Users.FindAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    // PUT: api/Users/... 
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(Guid id, User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;
         await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Users 
    [HttpPost]
    public async Task<IActionResult> PostUser(User user)
    {
        if (string.IsNullOrEmpty(user.FirstName))
        {
            return BadRequest("FirstName is required");
        }
        if (string.IsNullOrEmpty(user.LastName))
        {
            return BadRequest("LastName is required");
        }
        if (string.IsNullOrEmpty(user.Email))
        { 
            return BadRequest("Email is required");
        }
        else if (UserExists(user.Email))
        {
            return BadRequest("Email already exists");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    private bool UserIdExists(Guid id)
    {
        return _context.Users.Any(x => x.Id == id);
    }

    private bool UserExists(string email)
    {
        return _context.Users
                .Any(x => x.Email.ToLowerInvariant() == email.ToLowerInvariant());
    }
}
