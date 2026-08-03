using WebApplication1.Models.Enums;
using WebApplication1.Models.Entities;

namespace WebApplication1.Models.Entities;

public class Booking
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public Event? Event { get; set; }
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
    public DateTime BookingDate { get; set; }
    public BookingStatus Status { get; set; }
}