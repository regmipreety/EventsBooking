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
                 new User { Id = "1", Name = "Temporary User", Email = "tempuser@example.com", Phonenumber = "" },
                 new User { Id = "2", Name = "Temporary User 2", Email = "sun@gmail.com", Phonenumber = "123-456-7890" } 
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
                 new Event { Name = "Music Concert", Description = "A live music concert.", StartDate = DateTime.Now.AddDays(10), Price = 50, Location = "City Arena", Capacity = 2, Vendor = null },
                 new Event { Name = "Art Exhibition", Description = "An exhibition of modern art.", StartDate = DateTime.Now.AddDays(20), Price = 30, Location = "City Gallery", Capacity = 5, Vendor = null },
                 new Event { Name = "Momo Festival", Description = "A festival featuring various cuisines.", StartDate = DateTime.Now.AddDays(30), Price = 20, Location = "City Park", Capacity = 3, Vendor = null },
                 new Event { Name = "Food Festival", Description = "A festival featuring various cuisines.", StartDate = DateTime.Now.AddDays(30), Price = 20, Location = "City Park", Capacity = 2, Vendor = null },
             };

             context.Events.AddRange(events);
             context.SaveChanges();
         }
     }

     private static void SeedVendorProfiles(ApplicationDbContext context)
     {
         var vendorSeeds = new[]
         {
             new VendorProfile
             {
                 FirstName = "Vendor",
                 LastName = "1",
                 Description = "Description for Vendor 1",
                 EmailAddress = "vendor1@example.com",
                 Location = "New York",
                 Category = "Catering",
                 IsVerified = true,
                 PhoneNumber = "123-456-7890",
                 Rating = 4.7m
             },
             new VendorProfile
             {
                 FirstName = "Vendor",
                 LastName = "2",
                 Description = "Description for Vendor 2",
                 EmailAddress = "vendor2@example.com",
                 Location = "Los Angeles",
                 Category = "Photography",
                 IsVerified = true,
                 PhoneNumber = "098-765-4321",
                 Rating = 4.8m
             }
         };

         foreach (var vendorSeed in vendorSeeds)
         {
             var existingVendor = context.VendorProfile
                 .FirstOrDefault(v => v.EmailAddress == vendorSeed.EmailAddress);

             if (existingVendor == null)
             {
                 context.VendorProfile.Add(vendorSeed);
             }
             else
             {
                 existingVendor.FirstName = vendorSeed.FirstName;
                 existingVendor.LastName = vendorSeed.LastName;
                 existingVendor.Description = vendorSeed.Description;
                 existingVendor.EmailAddress = vendorSeed.EmailAddress;
                 existingVendor.Location = vendorSeed.Location;
                 existingVendor.Category = vendorSeed.Category;
                 existingVendor.IsVerified = vendorSeed.IsVerified;
                 existingVendor.PhoneNumber = vendorSeed.PhoneNumber;
                 existingVendor.Rating = vendorSeed.Rating;
             }
         }

         context.SaveChanges();
     }
}