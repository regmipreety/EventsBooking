using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;
using WebApplication1.Models.Enums;
namespace WebApplication1.Controllers;

public class BookingController : Controller
{
   private readonly IBookingService _bookingService;
   private readonly IBookingRepository _bookingRepository;
   private readonly ILogger<BookingController> _logger;

   private const string TempUserId= "temp-user-1";

   public BookingController(IBookingService bookingService, IBookingRepository bookingRepository, ILogger<BookingController> logger)
   {
       _bookingService = bookingService;
       _bookingRepository = bookingRepository;
       _logger = logger;
   }

public async Task<IActionResult> Index()
    {
        var bookings = await _bookingRepository.GetAllBookingsAsync();
        return View(bookings);
    }

    public async Task<IActionResult> MyBookings()
    {
        var bookings = await _bookingRepository.GetBookingsByUserIdAsync(TempUserId);
        return View(bookings);
    }

    public async Task<IActionResult> Details(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            return NotFound();
        }
        return View(booking);
    }

   [HttpPost]
   public async Task<IActionResult> BookEvent(int eventId)
   {
       var result = await _bookingService.BookEventAsync(eventId, TempUserId);
       if (!result.IsSuccess)
       {
           _logger.LogWarning("Failed to book event with ID {EventId} by user {UserId}: {ErrorMessage}", eventId, TempUserId, result.ErrorMessage);
           TempData["BookingError"] = result.ErrorMessage;
           return RedirectToAction("Index", "Events");
       }
        _logger.LogInformation("Successfully booked event with ID {EventId} by user {UserId}", eventId, TempUserId);
       TempData["BookingSuccess"] = "Booking successful!";
       return RedirectToAction("Index", "Events");
   }
         
   [HttpPost]
    public async Task<IActionResult> CancelBooking(int bookingId)
    {
        var result = await _bookingService.CancelBookingAsync(bookingId, TempUserId);
        if(!result.IsSuccess)
        {
            _logger.LogWarning("Failed to cancel booking with ID {BookingId} by user {UserId}: {ErrorMessage}", bookingId, TempUserId, result.ErrorMessage);
           
        }   
         TempData["BookingError"] = result.ErrorMessage;
            return RedirectToAction("MyBookings");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            return NotFound();
        }
        return View(booking);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookingStatus status)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        booking.Status = status;
        await _bookingRepository.UpdateBookingAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        TempData["BookingSuccess"] = "Booking updated successfully!";
        return RedirectToAction("MyBookings");
    }

}