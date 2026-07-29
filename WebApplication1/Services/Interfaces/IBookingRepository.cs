namespace WebApplication1.Services.Interfaces;
using WebApplication1.Models.Entities;

public interface IBookingRepository
{
    Task<List<Booking>> GetAllBookingsAsync();
    Task<Booking?>GetByIdAsync(int id);
    Task<List<Booking>> GetBookingsByEventIdAsync(int eventId);
    Task<List<Booking>> GetBookingsByUserIdAsync(string userId);
    Task<Booking?> GetBookingByIdAsync(int bookingId);
    Task AddBookingAsync(Booking booking);
    Task UpdateBookingAsync(Booking booking);
    Task DeleteBookingAsync(int bookingId);
    Task SaveChangesAsync();
}