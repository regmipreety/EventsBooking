using WebApplication1.Models.Entities;

namespace WebApplication1.Services.Interfaces;

public interface IUserService
{
    Task<User> GetOrCreateUserAsync(string username, string email, string phonenumber);
}