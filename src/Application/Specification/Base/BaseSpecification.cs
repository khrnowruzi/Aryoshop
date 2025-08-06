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

    public int Skip { get; private set; }

    public int Take { get; private set; }

    public bool IsPagingEnabled { get; private set; }

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

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    public IQueryable<TEntity> ApplyCriteria(IQueryable<TEntity> query)
    {
        if (Criteria != null)
        {
            query = query.Where(Criteria);
        }

        return query;
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
