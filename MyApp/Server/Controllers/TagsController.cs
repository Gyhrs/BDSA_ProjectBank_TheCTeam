namespace MyApp.Server.Controllers;

[Authorize]
[ApiController] 
[Route("api/[controller]")] 
[RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class TagsController : ControllerBase 
{
    private readonly ILogger<TagsController> _logger;
    private readonly ITagRepository _repository;

    public TagsController(ILogger<TagsController> logger, ITagRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)] 
    [HttpGet] 
    public async Task<ActionResult<IReadOnlyCollection<TagDTO>>> GetAllAsync()
    {
        var tags = await _repository.GetAllTagsAsync();
        return Ok(tags); 
    }
}