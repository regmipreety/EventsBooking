using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;
namespace WebApplication1.Controllers;

public class BookingController : Controller
{
   private readonly IBookingService _bookingService;
   
   private const string TempUserId= "temp-user-1";

   public BookingController(IBookingService bookingService)
   {
       _bookingService = bookingService;
   }

   [HttpPost]
   public async Task<IActionResult> BookEvent(int eventId)
   {
       var result = await _bookingService.BookEventAsync(eventId, TempUserId);
       if (result.IsSuccess)
       {
           TempData["BookingError"] = result.ErrorMessage;
           return RedirectToAction("Index", "Events");
       }

       TempData["BookingSuccess"] = "Booking successful!";
       return RedirectToAction("Index", "Events");
   }
       
   [HttpPost]
    public async Task<IActionResult> CancelBooking(int bookingId)
    {
        var result = await _bookingService.CancelBookingAsync(bookingId, TempUserId);
        TempData[result.IsSuccess ? "BookingSuccess" : "BookingError"] =result.IsSuccess? "Booking cancelled successfully!" : result.ErrorMessage;
        return RedirectToAction("MyBookings");
    }

    public async Task<IActionResult> MyBookings([FromServices] IBookingRepository bookingRepository)
    {
        var bookings = await bookingRepository.GetBookingsByUserIdAsync(TempUserId);
        return View(bookings);
    }

}