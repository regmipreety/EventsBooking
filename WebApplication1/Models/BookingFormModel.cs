
namespace WebApplication1.Models;

public class BookingFormModel
{
    public int EventId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}