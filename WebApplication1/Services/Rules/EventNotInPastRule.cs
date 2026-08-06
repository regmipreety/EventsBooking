using WebApplication1.Services.Interfaces;
using WebApplication1.Models.Entities;
namespace WebApplication1.Services.Rules;

public class EventNotInPastRule : IBookingRule
{
    public string? Validate(Event evt, string userId, List<Booking> existingBookings)
    {
        if (evt.StartDate < DateTime.Now)
        {
            return "Cannot book an event in the past.";
        }
        return null;
    }
}