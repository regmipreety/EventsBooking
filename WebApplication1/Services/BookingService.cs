using WebApplication1.Services.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.Entities;
namespace WebApplication1.Services;

public class BookingService: IBookingService
{
    private readonly IEventRepository _eventRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IEnumerable<IBookingRule> _bookingRules;
    private readonly ILogger<BookingService> _logger;

    public BookingService(IEventRepository eventRepository, IBookingRepository bookingRepository, IEnumerable<IBookingRule> bookingRules, ILogger<BookingService> logger)
    {
        _eventRepository = eventRepository;
        _bookingRepository = bookingRepository;
        _bookingRules = bookingRules;
        _logger = logger;
    }

    public async Task<BookingResult> BookEventAsync(int eventId, string userId)
    {
        var evt = await _eventRepository.GetEventByIdAsync(eventId);
        if (evt == null)
        {
            _logger.LogWarning("Attempt to book a non-existent event with ID {EventId} by user {UserId}", eventId, userId);
            return BookingResult.Fail("Event not found.");
        }

        var existingBookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);

        foreach (var rule in _bookingRules)
        {
            var validationMessage = rule.Validate(evt, userId, existingBookings);
            if (validationMessage != null)
            {
                _logger.LogWarning("Booking attempt failed for event {EventId} by user {UserId}: {ValidationMessage}", eventId, userId, validationMessage);
                return BookingResult.Fail(validationMessage);
            }
        }

        var booking = new Booking
        {
            EventId = eventId,
            UserId = userId,
            BookingDate = DateTime.UtcNow
        };

        await _bookingRepository.AddBookingAsync(booking);
        await _bookingRepository.SaveChangesAsync();

        return BookingResult.Success(booking.Id);
    }

    public async Task<BookingResult> CancelBookingAsync(int bookingId, string userId)
    {
        var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
        if (booking == null || booking.UserId != userId)
        {
            _logger.LogWarning("Attempt to cancel a non-existent booking with ID {BookingId} by user {UserId}", bookingId, userId);
            return BookingResult.Fail("Booking not found or you do not have permission to cancel this booking.");
        }

        await _bookingRepository.DeleteBookingAsync(bookingId);
        await _bookingRepository.SaveChangesAsync();

        return BookingResult.Success(bookingId);
    }
    
}