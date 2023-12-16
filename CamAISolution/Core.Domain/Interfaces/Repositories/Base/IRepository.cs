using System.Linq.Expressions;
using Core.Domain.Interfaces.Specifications.Repositories;
using Core.Domain.Models;

namespace Core.Domain.Interfaces.Repositories.Base;

public interface IRepository<T>
{
    public Task<T> GetByIdAsync(object key);
    public Task<PaginationResult<T>> GetAsync(IRepositorySpecification<T>? specification = null);
    public Task<PaginationResult<T>> GetAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string[]? includeProperties = null,
        bool disableTracking = true,
        bool takeAll = false,
        int pageIndex = 0,
        int pageSize = 5
    );
    public Task<T> AddAsync(T entity);
    public Task<int> CountAsync(Expression<Func<T, bool>>? expression = null);
    public T Update(T entity);
    public T Delete(T entity);
}
