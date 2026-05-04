namespace SimilaritySearch.Application.DTOs;

public class AdDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string BrandModel { get; set; } = string.Empty;
    public string Motor { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Location { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsReupload { get; set; }
    public string? ReuploadReason { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Summarize()
    {
        return $"Značka a model: {BrandModel}, Motor: {Motor}, UserId: {UserId}, Jméno uživatele: {UserName}, Lokace: {Location}, Cena: {Price} {Currency}, Vytvořeno: {CreatedAt}\n" +
               $"Popis: {Description}";
    }
    
}