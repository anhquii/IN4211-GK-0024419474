using EduHub.Application.Interfaces;
using EduHub.Domain.Entities;
using EduHub.Domain.Interfaces;

namespace EduHub.Application.Services;

public class EventService
{
    private readonly IEventRepository _eventRepository;
    private readonly INotificationServiceFactory _notificationFactory;
    public EventService(
        IEventRepository eventRepository, 
        INotificationServiceFactory notificationServiceFactory 

        )
    {
        _eventRepository = eventRepository ?? throw 
            new ArgumentNullException(nameof(eventRepository));
        _notificationFactory = notificationServiceFactory ?? throw
            new ArgumentNullException(nameof(notificationServiceFactory));
    }


    public Event CreateEvent(Guid clubId, string title, DateTime date, int capacity, string channel = "email")
    {
        var ev = Event.Create(clubId, title, date, capacity);
        var notificationService = _notificationFactory.Create(channel);
        _eventRepository.Add(ev);
        notificationService.Send($"New event: {ev.Title}");
        return ev;
    }

    public IReadOnlyList<Event> GetEventsByClub(Guid clubId)
    {
        return _eventRepository.GetByClubId(clubId);
    }

    public Event? GetEventById(Guid id)
    {
        return _eventRepository.GetById(id);
    }


    public bool DeleteEvent(Guid id)
    {
        return _eventRepository.Delete(id);
    }


}