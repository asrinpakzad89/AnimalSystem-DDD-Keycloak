using AnimalIdentifier.Application.Animals.ViewModels;
using AnimalIdentifier.Application.Commands;
using AnimalIdentifier.Application.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnimalIdentifier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public AnimalsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("Create")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(CreateAnimalCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("Update")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(UpdateAnimalCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("Delete")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(DeleteAnimalCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("GetById")]
    [Authorize]
    public async Task<ActionResult<AnimalDto>> GetById(GetAnimalByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("GetAll")]
    [Authorize]
    //[Authorize(Policy = "adminOrReader")]
    public async Task<ActionResult<List<AnimalDto>>> GetAll(CancellationToken cancellationToken)
    {
        GetAllAnimalsQuery query = new();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}

