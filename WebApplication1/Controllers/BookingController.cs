using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;
using WebApplication1.Models.Enums;
namespace WebApplication1.Controllers;

public class BookingController : Controller
{
   private readonly IBookingService _bookingService;
   private readonly IBookingRepository _bookingRepository;
   private readonly IUserRepository _userRepository;
   private readonly ILogger<BookingController> _logger;

   private const string TempUserId= "temp-user-1";

   public BookingController(IBookingService bookingService, IBookingRepository bookingRepository, IUserRepository userRepository, ILogger<BookingController> logger)
   {
       _bookingService = bookingService;
       _bookingRepository = bookingRepository;
       _userRepository = userRepository;
       _logger = logger;
   }

public async Task<IActionResult> Index()
    {
        var bookings = await _bookingRepository.GetAllBookingsAsync();
        return View(bookings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> Create(int eventId, string email, string? userId = null)
        => CreateBooking(eventId, email, userId);

    private async Task<IActionResult> CreateBooking(int eventId, string email, string? userId = null)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["BookingError"] = "Email is required to create a booking.";
            return RedirectToAction("Index", "Events");
        }

        var resolvedUserId = userId;
        if (string.IsNullOrWhiteSpace(resolvedUserId))
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                resolvedUserId = existingUser.Id;
            }
        }

        if (string.IsNullOrWhiteSpace(resolvedUserId))
        {
            TempData["BookingError"] = "No matching user was found for this email.";
            return RedirectToAction("Index", "Events");
        }

        var result = await _bookingService.BookEventAsync(eventId, resolvedUserId);
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Failed to create booking for event {EventId} for user {UserId}: {ErrorMessage}", eventId, resolvedUserId, result.ErrorMessage);
            TempData["BookingError"] = result.ErrorMessage;
            return RedirectToAction("Index", "Events");
        }

        TempData["BookingSuccess"] = "Booking created successfully!";
        return RedirectToAction("Index", "Events");
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
   [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(int bookingId)
    {
        var result = await _bookingService.CancelBookingAsync(bookingId, TempUserId);
        if(!result.IsSuccess)
        {
            _logger.LogWarning("Failed to cancel booking with ID {BookingId} by user {UserId}: {ErrorMessage}", bookingId, TempUserId, result.ErrorMessage);
            TempData["BookingError"] = result.ErrorMessage;
            return RedirectToAction("MyBookings");
        }

        TempData["BookingSuccess"] = "Booking cancelled successfully!";
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