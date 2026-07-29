using WebApplication1.Services.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.Entities;
namespace WebApplication1.Services;

public class BookingService: IBookingService
{
    private readonly IEventRepository _eventRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IEnumerable<IBookingRule> _bookingRules;

    public BookingService(IEventRepository eventRepository, IBookingRepository bookingRepository, IEnumerable<IBookingRule> bookingRules)
    {
        _eventRepository = eventRepository;
        _bookingRepository = bookingRepository;
        _bookingRules = bookingRules;
    }

    public async Task<BookingResult> BookEventAsync(int eventId, string userId)
    {
        var evt = await _eventRepository.GetEventByIdAsync(eventId);
        if (evt == null)
        {
            return BookingResult.Fail("Event not found.");
        }

        var existingBookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);

        foreach (var rule in _bookingRules)
        {
            var validationMessage = rule.Validate(evt, userId, existingBookings);
            if (validationMessage != null)
            {
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
            return BookingResult.Fail("Booking not found or you do not have permission to cancel this booking.");
        }

        await _bookingRepository.DeleteBookingAsync(bookingId);
        await _bookingRepository.SaveChangesAsync();

        return BookingResult.Success(bookingId);
    }
    
}