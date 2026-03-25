using Microsoft.EntityFrameworkCore;
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
        protected readonly DbContext _db;
        protected readonly DbSet<T> _table;

        protected Repository(DbContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public async Task<IResult<bool>> AnyAsync(Expression<Func<T, bool>>? expression = null)
        {
            try
            {
                bool any = expression != null ? await _table.AnyAsync(expression) : await _table.AnyAsync();
                return Result<bool>.Success(any);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure([ex.Message], 500);
            }
        }

        public async Task<IResult<int>> CountAsync(Expression<Func<T, bool>>? expression = null)
        {
            try
            {
                int count = expression != null ? await _table.CountAsync(expression) : await _table.CountAsync();
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure([ex.Message], 500);
            }
        }

        public async Task<IResult> CreateAsync(T entity)
        {
            try
            {
                await _table.AddAsync(entity);
                return Result.Success(201); // 201: Created
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        public async Task<IResult> CreateManyAsync(IEnumerable<T> entities)
        {
            try
            {
                await _table.AddRangeAsync(entities);
                return Result.Success(201); // 201: Created
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        public async Task<IResult> DeleteAsync(object id)
        {

            var result = await FindByIdAsync(id);
            if (result.IsSuccess)
            {
                return await DeleteAsync(result.Data);
            }
            return result;
        }

        public async Task<IResult> DeleteAsync(T entity)
        {
            try
            {
                await Task.Run(() => _table.Remove(entity));
                return Result.Success(204);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        public async Task<IResult> DeleteManyAsync(IEnumerable<T> entities)
        {
            try
            {
                await Task.Run(() => _table.RemoveRange(entities));
                return Result.Success(204);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, 500);
            }
        }

        public async Task<IResult> DeleteManyAsync(Expression<Func<T, bool>>? expression = null)
        {
            var entities = expression == null ? _table : _table.Where(expression);

            if (entities == null)
            {
                return Result.Failure("Entities Not found!", 404);
            }

            return await DeleteManyAsync(entities);
        }

        public async Task<IResult<T>> FindByIdAsync(object id)
        {
            var entity = await _table.FindAsync(id);

            if (entity == null)
            {
                return Result<T>.Failure(["Entity Not found!"], 404);
            }

            return Result<T>.Success(entity);
        }

        public Task<IResult<T>> FindFirstAsync(Expression<Func<T, bool>>? expression = null)
        {
            throw new NotImplementedException();
        }

        public Task<IResult<IEnumerable<T>>> FindManyAsync(Expression<Func<T, bool>>? expression = null, params string[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateManyAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
