using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services.Interfaces;
using WebApplication1.Models.Entities;
namespace WebApplication1.Controllers;

public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, IBookingRepository bookingRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _bookingRepository = bookingRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return View(users);
    }

    public async Task<IActionResult> Details(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found.", id);
            return NotFound();
        }
        return View(user);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(User user, int? eventId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Failed to create user due to invalid model state. Errors: {Errors}", string.Join(";", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return View(user);
            }

            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                user.Id = existingUser.Id;
                _logger.LogInformation("User with email {Email} already exists. Reusing user ID {UserId}.", user.Email, user.Id);
            }
            else
            {
                user.Id = string.IsNullOrWhiteSpace(user.Id) ? Guid.NewGuid().ToString() : user.Id;
                await _userRepository.AddUserAsync(user);
            }

            if (eventId.HasValue && eventId.Value > 0)
            {
                return RedirectToAction("Create", "Booking", new { eventId = eventId.Value, email = user.Email, userId = user.Id });
            }

            _logger.LogInformation("Successfully created a new user with ID {UserId}.", user.Id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a new user.");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the user. Please try again.");
            return View(user);
        }
    }
}