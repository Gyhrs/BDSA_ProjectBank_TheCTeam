using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MyApp.Shared;

namespace MyApp.Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class ProjectsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;
    private readonly IProjectRepository _repository;

    public ProjectsController(ILogger<ProjectsController> logger, IProjectRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    // Get all projects from DB
    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectDTO>> Get() => await _repository.ReadAsync();

    // Get project from projectID from DB
    [AllowAnonymous]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(ProjectDTO), 200)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
    {
        // Gets response (State) and Async ProjectDTO
        var (response, result) = _repository.ReadAsync(id);

        // Switch of the States. Return await result if found,
        //  otherwise corresponding http code.
        return response switch
        {
            State.Found => await result,
            State.NotFound => new NotFoundResult(),
            State.BadRequest => new BadRequestResult(),
            _ => throw new NotSupportedException("bruh")
        };
    }

    // Get projects from Name from DB
    [AllowAnonymous]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(IReadOnlyCollection<ProjectDTO>), 200)]
    [HttpGet("{name}")]
    public async Task<ActionResult<IReadOnlyCollection<ProjectDTO>>> Get(string name)
    {
        // Gets response (State) and Async ProjectDTO
        var (response, result) = _repository.ReadAsync(name);

        // Switch of the States. Return await result if found,
        //  otherwise corresponding http code.
        return response switch
        {
            // Hmmmmmmmmm is this legal?
            State.Found =>  Ok(await result),
            State.NotFound => new NotFoundResult(),
            State.BadRequest => new BadRequestResult(),
            _ => throw new NotSupportedException("bruh")
        };
    }

    // Get projects from Tags from DB
    [AllowAnonymous]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(ProjectDTO), 200)]
    [HttpGet("{tags}")]
    public async Task<ActionResult<IReadOnlyCollection<ProjectDTO>>> GetTags(string tags)
    {
        // Gets response (State) and Async ProjectDTO
        var (response, result) = _repository.ReadAsync(tags);

        // Switch of the States. Return await result if found,
        //  otherwise corresponding http code.
        return response switch
        {
            State.Found => Ok(await result),
            State.NotFound => new NotFoundResult(),
            State.BadRequest => new BadRequestResult(),
            _ => throw new NotSupportedException("bruh")
        };
    }
}
