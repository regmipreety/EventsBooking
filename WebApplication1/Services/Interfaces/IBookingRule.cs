namespace WebApplication1.Services.Interfaces;
using WebApplication1.Models.Entities;

public interface IBookingRule
{
    //returns true if the booking is valid, false otherwise
    string? Validate(Event evt, string userId, List<Booking> existingBookings);
}