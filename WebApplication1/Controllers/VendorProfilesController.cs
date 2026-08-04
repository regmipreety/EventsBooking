using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers;

public class VendorProfilesController : Controller
{
    private readonly IVendorProfileRepository _vendorProfileRepository;

    public VendorProfilesController(IVendorProfileRepository vendorProfileRepository)
    {
        _vendorProfileRepository = vendorProfileRepository;
    }

    // GET: VendorProfiles
    public async Task<IActionResult> Index()
    {
        var vendorProfiles = await _vendorProfileRepository.GetAllVendorProfilesAsync();
        return View(vendorProfiles);
    }

    // GET: VendorProfiles/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var vendorProfile = await _vendorProfileRepository.GetVendorProfileByIdAsync(id);
        if (vendorProfile == null)
        {
            return NotFound();
        }
        return View(vendorProfile);
    }

    // GET: VendorProfiles/Create
    public IActionResult Create()
    {
        return View();
    }
}