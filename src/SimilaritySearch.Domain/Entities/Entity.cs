using StockMapSvelte.Domain.Entities.Interfaces;

namespace StockMapSvelte.Domain.Entities;

public class Entity<TKey> : IEntity<TKey>
{ 
    public required TKey Id { get; set; }
}