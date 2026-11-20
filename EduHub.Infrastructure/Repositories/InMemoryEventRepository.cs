using EduHub.Domain.Entities;
using EduHub.Domain.Interfaces;

namespace EduHub.Infrastructure.Repositories;

public class InMemoryEventRepository : IEventRepository
{
    private readonly List<Event> _events = new();
    private readonly object _lock = new();

    public void Add(Event ev)
    {
        ArgumentNullException.ThrowIfNull(ev);

        lock (_lock)
        {
            _events.Add(ev);
        }
    }

    public Event? GetById(Guid id)
    {
        lock (_lock)
        {
            return _events.FirstOrDefault(e => e.Id == id);
        }
    }

    public IReadOnlyList<Event> GetByClubId(Guid clubId)
    {
        lock (_lock)
        {
            return _events.Where(e => e.ClubId == clubId).ToList();
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            var ev = _events.FirstOrDefault(e => e.Id == id);
            if (ev is null)
                return false;

            return _events.Remove(ev);
        }
    }
}