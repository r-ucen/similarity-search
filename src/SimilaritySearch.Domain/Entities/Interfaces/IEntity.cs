namespace SimilaritySearch.Domain.Entities.Interfaces;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}