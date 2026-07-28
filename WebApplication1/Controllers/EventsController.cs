using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;
namespace WebApplication1.Controllers;

public class EventsController : Controller
{
    private readonly IEventRepository _eventRepository;
    private readonly IBookingRule _bookingRule;

    public EventsController(IEventRepository eventRepository, IBookingRule bookingRule)
    {
        _eventRepository = eventRepository;
        _bookingRule = bookingRule;
    }

    // GET: Events
    public async Task<IActionResult> Index()
    {
        var events = await _eventRepository.GetAllEventsAsync();
        return View(events);
    }

    // GET: Events/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var evt = await _eventRepository.GetEventByIdAsync(id);
        if (evt == null)
        {
            return NotFound();
        }
        return View(evt);
    }
}