using WebApplication1.Models.Entities;
namespace WebApplication1.Data;

public static class DbInitializer
{
   public static void Seed(ApplicationDbContext context)
   {
      SeedUsers(context);
      SeedEvents(context);
      SeedVendorProfiles(context);

     }

     private static void SeedUsers(ApplicationDbContext context)
     {
         if (!context.Users.Any())
         {
             var users = new List<User>
             {
                 new User { Id = "temp-user-1", Name = "Temporary User", Email = "tempuser@example.com", Password = "" },
                 new User { Id = "temp-user-2", Name = "Temporary User 2", Email = "sun@gmail.com", Password = "123456" } 
             };

             context.Users.AddRange(users);
             context.SaveChanges();
         }
     }

     private static void SeedEvents(ApplicationDbContext context)
     {
         if (!context.Events.Any())
         {
             var events = new List<Event>
             {
                 new Event { Name = "Music Concert", Description = "A live music concert.", StartDate = DateTime.Now.AddDays(10), Price = 50, Location = "City Arena", Capacity = 2, VendorId = 1 },
                 new Event { Name = "Art Exhibition", Description = "An exhibition of modern art.", StartDate = DateTime.Now.AddDays(20), Price = 30, Location = "City Gallery", Capacity = 5, VendorId = 2 },
                 new Event { Name = "Momo Festival", Description = "A festival featuring various cuisines.", StartDate = DateTime.Now.AddDays(30), Price = 20, Location = "City Park", Capacity = 3, VendorId = 1 },
                 new Event { Name = "Food Festival", Description = "A festival featuring various cuisines.", StartDate = DateTime.Now.AddDays(30), Price = 20, Location = "City Park", Capacity = 2, VendorId = 1 },
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