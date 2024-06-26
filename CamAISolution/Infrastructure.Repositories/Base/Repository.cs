using System.Linq.Expressions;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Core.Domain.Specifications.Repositories;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.Repositories.Utils.QueryHelper;

namespace Infrastructure.Repositories.Base;

public class Repository<T>(CamAIContext context, IRepositorySpecificationEvaluator<T> specificationEvaluator)
    : IRepository<T>
    where T : class
{
    protected DbContext Context => context;

    public virtual async Task<T> AddAsync(T entity)
    {
        var result = await Context.Set<T>().AddAsync(entity);
        return result.Entity;
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? expression = null)
    {
        return expression == null ? await Context.Set<T>().CountAsync() : await Context.Set<T>().CountAsync(expression);
    }

    public virtual T Delete(T entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
            Context.Attach(entity);

        Context.Entry(entity).State = EntityState.Deleted;
        return entity;
    }

    public virtual async Task<PaginationResult<T>> GetAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string[]? includeProperties = null,
        bool disableTracking = true,
        bool takeAll = false,
        int pageIndex = 0,
        int pageSize = 5
    )
    {
        IQueryable<T> query = Context.Set<T>();
        var paginationResult = new PaginationResult<T> { TotalCount = await CountAsync(expression) };
        if (expression != null)
            query = query.Where(expression);

        if (disableTracking)
            query = query.AsNoTracking();

        if (includeProperties is { Length: > 0 })
        {
            foreach (var includeItem in includeProperties)
                query = query.Include(includeItem);

            query = query.AsSplitQuery();
        }

        if (takeAll)
        {
            if (orderBy != null)
                paginationResult.Values = await orderBy(query).ToListAsync();
            else
                paginationResult.Values = await SetDefaultOrderBy(query).ToListAsync();
        }
        else
        {
            paginationResult.PageIndex = pageIndex;
            if (orderBy == null)
            {
                query = SetDefaultOrderBy(query);
                paginationResult.Values = await query.Skip(pageSize * pageIndex).Take(pageSize).ToListAsync();
            }
            else
                paginationResult.Values = await orderBy(query).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        }

        paginationResult.PageIndex = pageIndex;
        paginationResult.PageSize = pageSize;
        return paginationResult;
    }

    public async Task<PaginationResult<T>> GetAsync(IRepositorySpecification<T>? specification = null)
    {
        if (specification == null)
            return new PaginationResult<T>();

        var query = specificationEvaluator.GetQuery(context.Set<T>(), specification);
        var count = await CountAsync(specification.Criteria);
        var data = await query.ToListAsync();
        return new PaginationResult<T>
        {
            PageIndex = specification.Skip / (specification.Take == 0 ? 1 : specification.Take),
            PageSize = data.Count,
            TotalCount = count,
            Values = data
        };
    }

    public virtual async Task<T?> GetByIdAsync(params object[] keys)
    {
        return await Context.Set<T>().FindAsync(keys);
    }

    public async Task<bool> IsExisted(object key)
    {
        var data = await context.Set<T>().FindAsync(key);
        if (data == null)
            return false;

        Context.Entry(data).State = EntityState.Detached;
        return true;
    }

    public virtual T Update(T entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            Context.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        return entity;
    }

    public IQueryable<IGrouping<TKey, T>> GroupEntity<TKey>(Expression<Func<T, TKey>> groupKey)
    {
        return context.Set<T>().GroupBy(groupKey);
    }

    public Task<bool> IsExisted(Expression<Func<T, bool>> predicate) =>
        context.Set<T>().Where(predicate).AsNoTracking().AnyAsync();
}
