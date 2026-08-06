namespace WebApplication1.Services.Interfaces;
using WebApplication1.Models.Entities;

public interface IEventRepository
{
   Task<List<Event>> GetAllEventsAsync();
   Task<Event?> GetEventByIdAsync(int eventId);
   Task AddEventAsync(Event evt);
   Task UpdateEventAsync(Event evt);
   Task DeleteEventAsync(int eventId);  
}