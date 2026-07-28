using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Entities;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

public class HomeController : Controller
{
    private readonly IVendorCatalog _vendorCatalog;

    public HomeController(IVendorCatalog vendorCatalog)
    {
        _vendorCatalog = vendorCatalog;
    }

    public IActionResult Index()
    {
        var model = new HomeViewModel
        {
            Vendors = _vendorCatalog.GetVendors()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Admin()
    {
        var model = new AdminViewModel
        {
            Vendors = _vendorCatalog.GetVendors()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Admin(AdminViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Vendors = _vendorCatalog.GetVendors();
            return View(model);
        }

        if (model.Vendor.BrowseImage is { Length: > 0 })
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid():N}_{Path.GetFileName(model.Vendor.BrowseImage.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = System.IO.File.Create(filePath);
            await model.Vendor.BrowseImage.CopyToAsync(stream);
            model.Vendor.ImagePath = $"/uploads/{fileName}";
        }

        _vendorCatalog.AddVendor(model.Vendor);
        model.Vendor = new VendorProfile();
        model.Vendors = _vendorCatalog.GetVendors();

        TempData["Message"] = "Vendor added successfully.";
        return RedirectToAction(nameof(Admin));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

