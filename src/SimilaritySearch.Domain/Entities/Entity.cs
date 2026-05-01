using SimilaritySearch.Domain.Entities.Interfaces;

namespace SimilaritySearch.Domain.Entities;

public class Entity<TKey> : IEntity<TKey>
{ 
    public required TKey Id { get; set; }
}