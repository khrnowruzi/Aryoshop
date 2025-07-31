using System.Linq.Expressions;
using Core.Interfaces;

namespace Application.Specification.Base;

public class Specification<TEntity>(Expression<Func<TEntity, bool>>? criteria)
    : ISpecification<TEntity>
{
    public Specification() : this(null) { }
    public Expression<Func<TEntity, bool>>? Criteria => criteria;
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

    public bool IsDistinct { get; private set; }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByAscending)
    {
        OrderBy = orderByAscending;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
    }

    protected void AddDistinct()
    {
        IsDistinct = true;
    }
}

public class Specification<TEntity, TResult>(Expression<Func<TEntity, bool>>? criteria)
    : Specification<TEntity>(criteria), ISpecification<TEntity, TResult>
{
    public Expression<Func<TEntity, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<TEntity, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}
