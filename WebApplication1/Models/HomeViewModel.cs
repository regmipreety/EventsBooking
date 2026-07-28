using WebApplication1.Models.Entities;
namespace WebApplication1.Models;

public class HomeViewModel
{
    public IReadOnlyList<VendorProfile> Vendors { get; set; } = Array.Empty<VendorProfile>();
}
