using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Services.Interfaces;
using WebApplication1.Models.Entities;
namespace WebApplication1.Controllers;

public class EventsController : Controller
{
    private readonly IEventRepository _eventRepository;
    private readonly IBookingRule _bookingRule;
    private readonly IVendorProfileRepository _vendorRepository;
    private readonly ILogger<EventsController> _logger;

    public EventsController(IEventRepository eventRepository, IBookingRule bookingRule, IVendorProfileRepository vendorRepository, ILogger<EventsController> logger)
    {
        _eventRepository = eventRepository;
        _bookingRule = bookingRule;
        _vendorRepository = vendorRepository;
        _logger = logger;
    }

    // GET: Events
    public async Task<IActionResult> Index()
    {
        var events = await _eventRepository.GetAllEventsAsync();
        _logger.LogInformation("Retrieved {EventCount} events for display on the index page.", events.Count);
        return View(events);
    }

    // GET: Events/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var evt = await _eventRepository.GetEventByIdAsync(id);
        if (evt == null)
        {
            _logger.LogWarning("Event with ID {EventId} not found.", id);
            return NotFound();
        }
        return View(evt);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {   
        await PopulateVendorsDropDownList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Event evt)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Failed to create event due to invalid model state.{Errors}",string.Join(";", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)    ));
            await PopulateVendorsDropDownList(evt.Vendor?.Id);
            return View(evt);
        }
        try
        {
            await SaveUploadedFile(evt);
            await _eventRepository.AddEventAsync(evt);

            _logger.LogInformation("Event with ID {EventId} created successfully.", evt.Id);
            TempData["SuccessMessage"] = "Event created successfully!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the event.");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the event. Please try again.");
            return View(evt);
        }

      
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var evt = await _eventRepository.GetEventByIdAsync(id);
        if (evt == null)
        {
            _logger.LogWarning("Event with ID {EventId} not found for editing.", id);
            return NotFound();
        }
        await PopulateVendorsDropDownList(evt.VendorId);
        return View(evt);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Event evt)
    {
        if (id != evt.Id)
        {
            _logger.LogWarning("Event ID mismatch: route ID {RouteId} does not match model ID {ModelId}.", id, evt.Id);
            return NotFound();
        }
         var existingEvent = await _eventRepository.GetEventByIdAsync(id);
        if (existingEvent == null)
        {
        _logger.LogWarning("Event with ID {EventId} not found for updating.", id);
            return NotFound();
        }
        if(!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(kvp => kvp.Value != null && kvp.Value.Errors.Count > 0)
                .Select(kvp => $"{kvp.Key}: {string.Join(", ", kvp.Value.Errors.Select(e => e.ErrorMessage))}");

            _logger.LogWarning("Failed to update event with ID {EventId} due to invalid model state. Errors: {Errors}",
                id, string.Join(" | ", errors));
            _logger.LogWarning("Failed to update event with ID {EventId} due to invalid model state.", id);
            return View(evt);
        }
        try
        {
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
            await SaveUploadedFile(evt);
            existingEvent.ImagePath = evt.ImagePath;
        }

        await _eventRepository.UpdateEventAsync(existingEvent);
        _logger.LogInformation("Event with ID {EventId} updated successfully.", id);
        TempData["SuccessMessage"] = "Event updated successfully!";
        return RedirectToAction(nameof(Index));
           
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving the uploaded file for event with ID {EventId}.", id);
            ModelState.AddModelError(string.Empty, "An error occurred while saving the uploaded file. Please try again.");
            return View(evt);
        }
       
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var evt = await _eventRepository.GetEventByIdAsync(id);
        if (evt == null)
        {
            _logger.LogWarning("Event with ID {EventId} not found for deletion.", id);
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
            _logger.LogWarning("Event with ID {EventId} not found for deletion.", id);
            return NotFound();
        }

        await _eventRepository.DeleteEventAsync(id);
        TempData["SuccessMessage"] = "Event deleted successfully!";
        return RedirectToAction(nameof(Index));
    }

    private async Task SaveUploadedFile(Event evt, IFormFile? file = null)
    {
        var imageFile = file ?? evt.BrowseImage;
        if(imageFile == null || imageFile.Length == 0) return;
    
        try
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            evt.ImagePath = "/uploads/" + uniqueFileName;
            _logger.LogInformation("Uploaded file for event {EventName} saved successfully at {FilePath}.", evt.Name, evt.ImagePath);
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving the uploaded file for event {EventName}.", evt.Name);
            throw; // Rethrow the exception to be handled by the calling method
        }
    }

    private async Task PopulateVendorsDropDownList(int? selectedVendorId = null)
    {
        var vendors = await _vendorRepository.GetAllVendorProfilesAsync();
        
        ViewBag.Vendors = vendors.Select(v => new SelectListItem
        {
            Value = v.Id.ToString(),
            Text = v.FullName,
            Selected = (v.Id == selectedVendorId)
        }).ToList();
    }


}