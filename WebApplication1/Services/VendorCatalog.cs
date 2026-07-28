using WebApplication1.Models.Entities;

namespace WebApplication1.Services;

public class VendorCatalog : IVendorCatalog
{
    private readonly List<VendorProfile> _vendors = new();

    public VendorCatalog()
    {
        Seed();
    }

    public IReadOnlyList<VendorProfile> GetVendors()
    {
        return _vendors.OrderByDescending(v => v.Id).ToList();
    }

    public void AddVendor(VendorProfile vendor)
    {
        ArgumentNullException.ThrowIfNull(vendor);

        _vendors.Add(new VendorProfile
        {
            Id = _vendors.Count + 1,
            FirstName = vendor.FirstName.Trim(),
            LastName = vendor.LastName.Trim(),
            EmailAddress = vendor.EmailAddress.Trim(),
            PhoneNumber = vendor.PhoneNumber.Trim(),
            Category = vendor.Category.Trim(),
            Location = vendor.Location.Trim(),
            Description = vendor.Description.Trim(),
            Price = vendor.Price,
            Availability = vendor.Availability,
            ImagePath = vendor.ImagePath,
            Rating = vendor.Rating,
            IsVerified = vendor.IsVerified
        });
    }

    private void Seed()
    {
        _vendors.AddRange(new[]
        {
            new VendorProfile
            {
                Id = 1,
                FirstName = "Willowmere",
                LastName = "Estate",
                EmailAddress = "hello@willowmere.co.uk",
                PhoneNumber = "+44 7700 900123",
                Category = "Venue",
                Location = "Berkshire",
                Description = "Historic manor house for luxury weddings",
                Price = 3200,
                Availability = DateTime.Today.AddDays(7),
                ImagePath = "/uploads/willowmere.jpg",
                Rating = 4.9m,
                IsVerified = true
            },
            new VendorProfile
            {
                Id = 2,
                FirstName = "Maren",
                LastName = "Cole",
                EmailAddress = "maren@studio.co.uk",
                PhoneNumber = "+44 7700 900456",
                Category = "Photographer",
                Location = "London",
                Description = "Editorial-style storytelling for every celebration",
                Price = 850,
                Availability = DateTime.Today.AddDays(14),
                ImagePath = "/uploads/maren.jpg",
                Rating = 5.0m,
                IsVerified = true
            },
            new VendorProfile
            {
                Id = 3,
                FirstName = "Studio",
                LastName = "Amara",
                EmailAddress = "studio@amara.co.uk",
                PhoneNumber = "+44 7700 900789",
                Category = "Makeup artist",
                Location = "Manchester",
                Description = "Bridal glamour and event-ready beauty",
                Price = 180,
                Availability = DateTime.Today.AddDays(21),
                ImagePath = "/uploads/amara.jpg",
                Rating = 4.8m,
                IsVerified = true
            }
        });
    }
}
