using Domain.Entities.Base;

namespace Domain.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task<IReadOnlyList<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> spec);
    Task<IReadOnlyList<TResult>> GetAllWithSpecificationAsync<TResult>(ISpecification<TEntity, TResult> spec);
    Task<int> CountAsync(ISpecification<TEntity> spec);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<bool> SaveAllAsync();
}
