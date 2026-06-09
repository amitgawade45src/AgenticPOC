namespace BankService.Models;

/// <summary>
/// The request bound from the "Apply to open account" form. The uploaded document
/// is received as a multipart <see cref="IFormFile"/>.
/// </summary>
public sealed class OpenAccountRequest
{ 
    public string FullName { get; set; } = string.Empty;
    public IFormFile Document { get; set; } = default!;
}
