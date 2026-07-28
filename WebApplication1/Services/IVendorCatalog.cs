using WebApplication1.Models.Entities;

namespace WebApplication1.Services;

public interface IVendorCatalog
{
    IReadOnlyList<VendorProfile> GetVendors();
    void AddVendor(VendorProfile vendor);
}
