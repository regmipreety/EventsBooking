using WebApplication1.Models.Entities;
namespace WebApplication1.Services.Interfaces;
public interface IVendorProfileRepository
{
    Task<VendorProfile?> GetVendorProfileByIdAsync(int vendorId);
    Task<List<VendorProfile>> GetAllVendorProfilesAsync();
    Task AddVendorProfileAsync(VendorProfile vendorProfile);
    Task UpdateVendorProfileAsync(VendorProfile vendorProfile);
    Task DeleteVendorProfileAsync(int vendorId);
}