namespace WebApplication1.Repositories;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Services.Interfaces;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllEventsAsync()
    {
        return await _context.Events
        .Include(e => e.Vendor)
        .OrderBy(e => e.StartDate)
        .ToListAsync();
    }

    public async Task<Event?> GetEventByIdAsync(int eventId)
    {
        return await _context.Events
        .Include(e => e.Vendor)
        .Include(e => e.Bookings)
        .FirstOrDefaultAsync(e => e.Id == eventId);
    }

    public async Task AddEventAsync(Event evt)
    {
        _context.Events.Add(evt);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEventAsync(Event evt)
    {
        _context.Events.Update(evt);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEventAsync(int eventId)
    {
        var evt = await _context.Events.FindAsync(eventId);
        if (evt != null)
        {
            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();
        }
    }
}