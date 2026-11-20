using EduHub.Application.Services;
using EduHub.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EduHub.API.Controllers;

[ApiController]
[Route("api")]
public class EventsController : ControllerBase
{
    private readonly EventService _eventService;

    public EventsController(EventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet("clubs/{clubId:guid}/events")]
    [ProducesResponseType(typeof(IReadOnlyList<Event>), StatusCodes.Status200OK)]
    public IActionResult GetByClub(Guid clubId)
    {
        var events = _eventService.GetEventsByClub(clubId);
        return Ok(events);
    }

    [HttpGet("events/{id:guid}")]
    [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var ev = _eventService.GetEventById(id);
        return ev is null ? NotFound() : Ok(ev);
    }

    [HttpPost("clubs/{clubId:guid}/events")]
    [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create(Guid clubId, [FromBody] CreateEventRequest request)
    {
        try
        {
            var ev = _eventService.CreateEvent(
                clubId,
                request.Title,
                request.Date,
                request.Capacity,
                request.Channel);

            return CreatedAtAction(nameof(GetById), new { id = ev.Id }, ev);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("events/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var deleted = _eventService.DeleteEvent(id);
        return deleted ? NoContent() : NotFound();
    }
}

public record CreateEventRequest(string Title, DateTime Date, int Capacity, string Channel = "email");