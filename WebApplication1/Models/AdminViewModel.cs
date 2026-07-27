namespace WebApplication1.Models;

public class AdminViewModel
{
    public VendorProfile Vendor { get; set; } = new();

    public IReadOnlyList<VendorProfile> Vendors { get; set; } = Array.Empty<VendorProfile>();
}
