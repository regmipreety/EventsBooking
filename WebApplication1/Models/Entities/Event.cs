using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication1.Models.Entities;

public class Event
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Event name is required.")]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public int VendorId { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;
    public VendorProfile Vendor { get; set; } = new();

    [NotMapped]
    public IFormFile? BrowseImage { get; set; }
    public string? ImagePath { get; set; }
    public List<Booking> Bookings { get; set; } = new();
}
