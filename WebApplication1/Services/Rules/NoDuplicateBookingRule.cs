using WebApplication1.Services.Interfaces;
using WebApplication1.Models.Entities;
namespace WebApplication1.Services.Rules;

public class NoDuplicateBookingRule: IBookingRule
{

  public string? Validate(Event evt, string userId, List<Booking> existingBookings)
    {
        var hasDuplicateBooking = existingBookings.Any(b => b.EventId == evt.Id && b.UserId == userId);
        if (hasDuplicateBooking)
        {
            return "You have already booked this event.";
        }
        return null;
    }
  
}