using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MyApp.Shared;

namespace MyApp.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class UsersController : ControllerBase // Inherits from ControllerBase. Controllerbase does not have view support (from MVC). If we want view support we can inherit from Controller. Controllerbase has http codes Ok = 200, etc.
{
    // Dependency injection
    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _repository;

    public UsersController(ILogger<UsersController> logger, IUserRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    // Get all users from DB
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<UserDTO>>> GetAll()
    {
        var users = await _repository.GetAllUsersAsync();
        return Ok(users);
    }

    // Get user from userEmail from DB
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserDTO>> GetFromEmail(string email)
    {
        if (email.Length == 0)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                return BadRequest("Invalid email address");
            }
        }

        var user = await _repository.GetUserFromEmailAsync(email);

        if (user == null)
        {
            return NotFound("No user found with specified email");
        }

        return Ok(user);
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("students/{id}")]
    public async Task<ActionResult<IReadOnlyCollection<UserDTO>>> GetStudentsFromProjectId(int id)
    {
        if (id < 0)
        {
            return BadRequest("ids can't be negative");
        }

        var students = await _repository.GetStudentsFromProjectIDAsync(id);

        if (students.Count == 0)
        {
            return NotFound("No students with specified projectId");
        }
        return Ok(students);
    }

    [AllowAnonymous]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(UserDTO), 200)]
    [HttpGet("supervisors/{id}")]
    public async Task<ActionResult<IReadOnlyCollection<UserDTO>>> GetSupervisorsFromProjectId(int id)
    {
         if (id < 0)
        {
            return BadRequest("ids can't be negative");
        }

        var supervisors = await _repository.GetSupervisorsFromProjectIDAsync(id);

        if (supervisors.Count == 0)
        {
            return NotFound("No students with specified projectId");
        }
        return Ok(supervisors);
    }
}