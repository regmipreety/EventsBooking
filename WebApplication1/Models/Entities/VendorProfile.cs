using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Models.Entities;

public class VendorProfile
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email address")]
    public string EmailAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; } = string.Empty;


    [Display(Name = "Category")]
    public string Category { get; set; } = string.Empty;


    [Display(Name = "Location")]
    public string Location { get; set; } = string.Empty;


    [Display(Name = "Description")]
    public string Description { get; set; } = string.Empty;


    [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5")]
    [Display(Name = "Rating")]
    public decimal Rating { get; set; } = 4.8m;

    [Display(Name = "Verified vendor")]
    public bool IsVerified { get; set; } = true;

    [Display(Name = "Full name")]
    public string FullName => $"{FirstName} {LastName}".Trim();
}
