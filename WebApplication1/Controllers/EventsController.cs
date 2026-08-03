using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;
using WebApplication1.Models.Entities;
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

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Event evt)
    {
        if (ModelState.IsValid)
        {
            return View(evt);
        }

        await SaveUploadedFile(evt);
        await _eventRepository.AddEventAsync(evt);

        TempData["SuccessMessage"] = "Event created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var evt = await _eventRepository.GetEventByIdAsync(id);
        if (evt == null)
        {
            return NotFound();
        }
        return View(evt);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Event evt)
    {
        if (id != evt.Id)
        {
            return NotFound();
        }
         var existingEvent = await _eventRepository.GetEventByIdAsync(id);
        if (existingEvent == null)
        {
            return NotFound();
        }
        if(!ModelState.IsValid)
        {
            return View(evt);
        }
        existingEvent.Name = evt.Name;
        existingEvent.Description = evt.Description;
        existingEvent.StartDate = evt.StartDate;
        existingEvent.EndDate = evt.EndDate;
        existingEvent.Price = evt.Price;
        existingEvent.VendorId = evt.VendorId;
        existingEvent.Capacity = evt.Capacity;
        existingEvent.Location = evt.Location;  

        if(evt.BrowseImage != null)
        {
            await SaveUploadedFile(existingEvent);
        }

        await _eventRepository.UpdateEventAsync(existingEvent);
        TempData["SuccessMessage"] = "Event updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var evt = await _eventRepository.GetEventByIdAsync(id);
        if (evt == null)
        {
            return NotFound();
        }
        return View(evt);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var evt = await _eventRepository.GetEventByIdAsync(id);
        if (evt == null)
        {
            return NotFound();
        }

        await _eventRepository.DeleteEventAsync(id);
        TempData["SuccessMessage"] = "Event deleted successfully!";
        return RedirectToAction(nameof(Index));
    }

    private async Task SaveUploadedFile(Event evt)
    {
        if (evt.BrowseImage != null && evt.BrowseImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + evt.BrowseImage.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await evt.BrowseImage.CopyToAsync(fileStream);
            }

            evt.ImagePath = "/uploads/" + uniqueFileName;
        }
    }


}