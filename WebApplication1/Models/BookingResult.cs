namespace WebApplication1.Models;

public class BookingResult
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public int BookingId { get; set; }

    public static BookingResult Fail(string message)=>new() { IsSuccess = false, ErrorMessage = message };
    public static BookingResult Success(int bookingId) => new() { IsSuccess = true, BookingId = bookingId };
}