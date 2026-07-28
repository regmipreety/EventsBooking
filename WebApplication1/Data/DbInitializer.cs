using WebApplication1.Models.Entities;
namespace WebApplication1.Data;

public static class DbInitializer
{
   public static void Seed(ApplicationDbContext context)
   {
      SeedEvents(context);
      SeedVendorProfiles(context);

     }

     private static void SeedEvents(ApplicationDbContext context)
     {
         if (!context.Events.Any())
         {
             var events = new List<Event>
             {
                 new Event { Name = "Music Concert", Description = "A live music concert.", StartDate = DateTime.Now.AddDays(10), Price = 50.00m },
                 new Event { Name = "Art Exhibition", Description = "An exhibition of modern art.", StartDate = DateTime.Now.AddDays(20), Price = 30.00m },
                 new Event { Name = "Food Festival", Description = "A festival featuring various cuisines.", StartDate = DateTime.Now.AddDays(30), Price = 20.00m }
             };

             context.Events.AddRange(events);
             context.SaveChanges();
         }
     }

     private static void SeedVendorProfiles(ApplicationDbContext context)
     {
         if (!context.VendorProfile.Any())
         {
             var vendors = new List<VendorProfile>
             {
                 new VendorProfile { FirstName = "Vendor", LastName = "1", Description = "Description for Vendor 1", EmailAddress = "vendor1@example.com" },
                 new VendorProfile { FirstName = "Vendor", LastName = "2", Description = "Description for Vendor 2", EmailAddress = "vendor2@example.com" }
             };

             context.VendorProfile.AddRange(vendors);
             context.SaveChanges();
         }
     }
}