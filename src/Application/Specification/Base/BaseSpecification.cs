using System.Linq.Expressions;
using Core.Interfaces;

namespace Application.Specification.Base;

public class BaseSpecification<TEntity>(Expression<Func<TEntity, bool>>? criteria)
    : ISpecification<TEntity>
{
    public BaseSpecification() : this(null) { }
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

public class BaseSpecification<TEntity, TResult>(Expression<Func<TEntity, bool>>? criteria)
    : BaseSpecification<TEntity>(criteria), ISpecification<TEntity, TResult>
{
    public Expression<Func<TEntity, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<TEntity, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}
