using System.ComponentModel.DataAnnotations.Schema;
using Pgvector;

namespace SimilaritySearch.Domain.Entities;

[Table(nameof(Ad))]
public class Ad : Entity<Guid>
{
    public required string UserId { get; set; }
    public required string UserName { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public required string Location { get; set; }
    public required decimal Price { get; set; }
    public required string Currency { get; set; }
    
    public required string Description { get; set; }
    
    // EmbeddingGemma-300M embedding output size 768
    [Column(TypeName = "vector(768)")]
    public Vector? DescriptionEmbedding { get; set; }

    public bool IsReupload { get; set; } = false;
    public string? ReuploadReason { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}