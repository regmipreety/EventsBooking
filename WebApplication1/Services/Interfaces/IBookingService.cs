using WebApplication1.Models;
namespace WebApplication1.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResult> BookEventAsync(int eventId, string userId);
    Task<BookingResult> CancelBookingAsync(int bookingId, string userId);
}