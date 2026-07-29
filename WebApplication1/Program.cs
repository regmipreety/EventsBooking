using WebApplication1.Services;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Services.Interfaces;
using WebApplication1.Repositories;
using WebApplication1.Services.Rules;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookingRule, CapacityRule>();
builder.Services.AddScoped<IBookingRule, NoDuplicateBookingRule>();
builder.Services.AddScoped<IBookingRule, EventNotInPastRule>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IVendorCatalog, VendorCatalog>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Seed(dbContext);
}

app.Run();
