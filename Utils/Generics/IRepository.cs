using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utils.Responses;

namespace Utils.Generics
{
    public interface IRepository<T> where T : class
    {
        Task<IResult> CreateAsync(T entity);
        Task<IResult> CreateManyAsync(IEnumerable<T> entities);

        Task<IResult<IEnumerable<T>>> FindManyAsync(Expression<Func<T, bool>>? expression = null, params string[] includes);
        Task<IResult<T>> FindByIdAsync(object id);
        Task<IResult<T>> FindFirstAsync(Expression<Func<T, bool>>? expression = null);

        Task<IResult> UpdateAsync(T entity);
        Task<IResult> UpdateManyAsync(IEnumerable<T> entities);

        Task<IResult> DeleteAsync(object id);
        Task<IResult> DeleteAsync(T entity);
        Task<IResult> DeleteManyAsync(IEnumerable<T> entities);
        Task<IResult> DeleteManyAsync(Expression<Func<T, bool>>? expression = null);

        Task<IResult<int>> CountAsync(Expression<Func<T, bool>>? expression = null);
        Task<IResult<bool>> AnyAsync(Expression<Func<T, bool>>? expression = null);
    }

    public abstract class Repository<T> : IRepository<T> where T : class
    {

    }
}
