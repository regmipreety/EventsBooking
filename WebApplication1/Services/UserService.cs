using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.Entities;
namespace WebApplication1.Services.Interfaces;

public class UserService:IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<User> GetOrCreateUserAsync(string username, string email, string phonenumber)
    {
        try
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                _logger.LogInformation("User with email {Email} already exists. No new user created.", email);
                return existingUser;
            }

            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = username,
                Email = email,
                Phonenumber = phonenumber
            };

            await _userRepository.AddUserAsync(newUser);
            return newUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing user with email {Email}.", email);
            throw;
        }
    }
}


       