using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;
using WebApplication1.Models.Enums;
using WebApplication1.Models;
namespace WebApplication1.Controllers;

public class BookingController : Controller
{
   private readonly IBookingService _bookingService;
   private readonly IBookingRepository _bookingRepository;
   private readonly IUserRepository _userRepository;
   private readonly IEventRepository _eventRepository;
   private readonly ILogger<BookingController> _logger;

   private readonly IUserService _userService;

   public BookingController(IBookingService bookingService, IBookingRepository bookingRepository, IUserRepository userRepository, IEventRepository eventRepository, ILogger<BookingController> logger, IUserService userService)
   {
       _bookingService = bookingService;
       _bookingRepository = bookingRepository;
       _userRepository = userRepository;
       _eventRepository = eventRepository;
       _logger = logger;
       _userService = userService;
   }

public async Task<IActionResult> Index()
    {
        var bookings = await _bookingRepository.GetAllBookingsAsync();
        return View(bookings);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int eventId)
    {
        var evt = await _eventRepository.GetEventByIdAsync(eventId);
        if(evt == null)
        {
            TempData["BookingError"] = "Event not found.";
            return RedirectToAction("Index", "Home");

        }
        if(evt.StartDate < DateTime.UtcNow)
        {
            TempData["BookingError"] = "Cannot book past events.";
            return RedirectToAction("Index", "Home");
        }
        var existingBookings = await _bookingRepository.GetBookingsByEventIdAsync(eventId);
        var confirmedBookingsCount = existingBookings.Count(b => b.Status == BookingStatus.Confirmed);
        if(confirmedBookingsCount >= evt.Capacity)
        {
            TempData["BookingError"] = "Event is fully booked.";
            return RedirectToAction("Index", "Home");
        }
        var model = new BookingFormModel
        {
            EventId = eventId
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingFormModel model)
    {
        if(string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Username))
        {
            ModelState.AddModelError(string.Empty, "Email and Username are required.");
            return RedirectToAction("Index", "Events");
        }

        try{
            var user = await _userService.GetOrCreateUserAsync(model.Username, model.Email, model.PhoneNumber);
            var result = await _bookingService.BookEventAsync(model.EventId, user.Id);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return RedirectToAction("Index", "Events");
            }

            TempData["BookingSuccess"] = "Booking successful!";
            return RedirectToAction("Index", "Events");
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a booking for event {EventId} by user {Email}", model.EventId, model.Email);
            ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
            return RedirectToAction("Index", "Events");
        }
    }

    public async Task<IActionResult> MyBookings(string userId)
    {
        var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
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
   public async Task<IActionResult> BookEvent(int eventId, string userId)
   {

       var result = await _bookingService.BookEventAsync(eventId, userId);
       if (!result.IsSuccess)
       {
           _logger.LogWarning("Failed to book event with ID {EventId} by user {UserId}: {ErrorMessage}", eventId, userId, result.ErrorMessage);
           TempData["BookingError"] = result.ErrorMessage;
           return RedirectToAction("Index", "Events");
       }
        _logger.LogInformation("Successfully booked event with ID {EventId} by user {UserId}", eventId, userId);
       TempData["BookingSuccess"] = "Booking successful!";
       return RedirectToAction("Index", "Events");
   }
         
   [HttpPost]
   [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(int bookingId, string userId)
    {
        var result = await _bookingService.CancelBookingAsync(bookingId, userId);
        if(!result.IsSuccess)
        {
            _logger.LogWarning("Failed to cancel booking with ID {BookingId} by user {UserId}: {ErrorMessage}", bookingId, userId, result.ErrorMessage);
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
    [HttpGet]
    public IActionResult SearchByEmail()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> GetBookingByEmail(string userEmail)
    {
        var bookings = await _bookingRepository.GetBookingsByUserEmailAsync(userEmail);

        return RedirectToAction("MyBookings", new { userId = bookings.FirstOrDefault()?.UserId });
    }
}
        
     
    

