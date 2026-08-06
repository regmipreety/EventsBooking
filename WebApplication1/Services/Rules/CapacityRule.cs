namespace WebApplication1.Services.Rules;
using WebApplication1.Models.Entities;
using WebApplication1.Services.Interfaces;
using WebApplication1.Models.Enums;

public class CapacityRule : IBookingRule
{
    public string? Validate(Event evt, string userId, List<Booking> existingBookings)
    {
        var confirmedCount = existingBookings.Count(b => b.Status == BookingStatus.Confirmed);
        if (confirmedCount >= evt.Capacity)
        {
            return "Event is fully booked.";
        }
        return null;
    }
}