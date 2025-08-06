using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    Expression<Func<TEntity, object>>? OrderBy { get; }
    Expression<Func<TEntity, object>>? OrderByDescending { get; }
    bool IsDistinct { get; }
    int Skip { get; }
    int Take { get; }
    bool IsPagingEnabled { get; }
    IQueryable<TEntity> ApplyCriteria(IQueryable<TEntity> query);
}

public interface ISpecification<TEntity, TResult> : ISpecification<TEntity>
{
    Expression<Func<TEntity, TResult>>? Select { get; }
}
