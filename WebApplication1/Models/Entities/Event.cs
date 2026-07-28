namespace WebApplication1.Models.Entities;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public int VendorId { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;
    public VendorProfile Vendor { get; set; } = new();

    public List<Booking> Bookings { get; set; } = new();
}
