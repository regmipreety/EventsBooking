using WebApplication1.Models.Entities;
namespace WebApplication1.Services.Interfaces;


public interface IBookingRule
{
    //returns true if the booking is valid, false otherwise
    string? Validate(Event evt, string userId, List<Booking> existingBookings);
}