using Application.DTO.Collaborators;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers;

[Route("api/collaborators")]
[ApiController]
public class CollaboratorController : ControllerBase
{
    private readonly ICollaboratorService _collabService;

    public CollaboratorController(ICollaboratorService collabService)
    {
        _collabService = collabService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Guid>>> Get()
    {
        var collaborators = await _collabService.GetAll();

        return collaborators.ToActionResult();
    }

    [HttpGet("{collaboratorId}")]
    public async Task<ActionResult<CollaboratorDTO>> GetById(Guid collaboratorId)
    {
        var collaborator = await _collabService.GetById(collaboratorId);

        return collaborator.ToActionResult();
    }

    [HttpGet("details")]
    public async Task<ActionResult<IEnumerable<CollabDetailsDTO>>> GetAllInfo()
    {
        var collaborators = await _collabService.GetAllInfo();

        return collaborators.ToActionResult();
    }

    [HttpGet("{collaboratorId}/details")]
    public async Task<ActionResult<CollabDetailsDTO>> GetDetailsById(Guid collaboratorId)
    {
        var collaborator = await _collabService.GetDetailsById(collaboratorId);

        return collaborator.ToActionResult();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Guid>>> FindBy([FromQuery] string? name, [FromQuery] string? surname)
    {
        if (name == null && surname == null)
            return BadRequest("Please insert at least a name or surname");

        IEnumerable<Guid> collabIds;

        if (surname == null)
        {
            collabIds = await _collabService.GetByNames(name);
        }
        else if (name == null)
        {
            collabIds = await _collabService.GetBySurnames(surname);
        }
        else
        {
            collabIds = await _collabService.GetByNamesAndSurnames(name, surname);
        }

        if (collabIds.Any())
            return Ok(collabIds);

        else return NotFound();
    }
}