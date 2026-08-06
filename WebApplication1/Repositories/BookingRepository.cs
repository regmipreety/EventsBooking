namespace WebApplication1.Repositories;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Services.Interfaces; 

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        return await _context.Bookings.Include(b => b.Event).ToListAsync();
    }
    public async Task<Booking?> GetByIdAsync(int id)
        => await _context.Bookings
            .Include(b => b.Event)
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);
    
    public async Task<List<Booking>> GetBookingsByEventIdAsync(int eventId)
    {
        return await _context.Bookings.Include(b => b.Event)
            .Where(b => b.EventId == eventId)
            .ToListAsync();
    }  

    public async Task<List<Booking>> GetBookingsByUserIdAsync(string userId)
    {
        return await _context.Bookings
            .Include(b => b.Event)
            .Include(b => b.User)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    } 

     public async Task<List<Booking>> GetBookingsByUserEmailAsync(string userEmail)
    {
        return await _context.Bookings.Include(b => b.User)
            .Where(b => b.User != null && b.User.Email == userEmail)
            .ToListAsync();
    } 

    public async Task<Booking?> GetBookingByIdAsync(int bookingId)
    {
        return await _context.Bookings.FindAsync(bookingId);
    }

    public async Task AddBookingAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookingAsync(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}