using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MyApp.Shared;

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
    public async Task<ActionResult<IReadOnlyCollection<TagDTO>>> GetAll()
    {
        var tags = await _repository.GetAllTags();
        return Ok(tags); 
    }
}