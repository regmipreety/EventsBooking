using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
using WebApplication1.Services.Interfaces;
namespace WebApplication1.Repositories;

public class VendorProfileRepository : IVendorProfileRepository
{
    private readonly ApplicationDbContext _context;

    public VendorProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VendorProfile?> GetVendorProfileByIdAsync(int vendorId)
    {
        return await _context.VendorProfile.FindAsync(vendorId);
    }

    public async Task<List<VendorProfile>> GetAllVendorProfilesAsync()
    {
        return await _context.VendorProfile.ToListAsync();
    }

    public async Task AddVendorProfileAsync(VendorProfile vendorProfile)
    {
        _context.VendorProfile.Add(vendorProfile);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVendorProfileAsync(VendorProfile vendorProfile)
    {
        _context.VendorProfile.Update(vendorProfile);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVendorProfileAsync(int vendorId)
    {
        var vendorProfile = await _context.VendorProfile.FindAsync(vendorId);
        if (vendorProfile != null)
        {
            _context.VendorProfile.Remove(vendorProfile);
            await _context.SaveChangesAsync();
        }
    }
}