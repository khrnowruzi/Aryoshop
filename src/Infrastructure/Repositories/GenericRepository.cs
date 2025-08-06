using Core.Entities.Base;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GenericRepository<TEntity, TKey>(StoreContext context) : IGenericRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    private readonly StoreContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task<int> CountAsync(ISpecification<TEntity> spec)
    {
        var query = _dbSet.AsQueryable();

        query = spec.ApplyCriteria(query);

        return await query.CountAsync();
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> spec)
    {
        return await ApplySpecification(spec).AsNoTracking().ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> GetAllWithSpecificationAsync<TResult>(
        ISpecification<TEntity, TResult> spec
        )
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity, TKey>.GetQuery(_dbSet.AsQueryable(), spec);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> spec)
    {
        return SpecificationEvaluator<TEntity, TKey>.GetQuery<TEntity, TResult>(_dbSet.AsQueryable(), spec);
    }
}
