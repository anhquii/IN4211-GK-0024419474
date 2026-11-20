using EduHub.Domain.Entities;
namespace EduHub.Domain.Interfaces;


public interface IEventRepository
{
    void Add(Event ev);
    Event? GetById(Guid id);
    IReadOnlyList<Event> GetByClubId(Guid clubId);
    bool Delete(Guid id);






}