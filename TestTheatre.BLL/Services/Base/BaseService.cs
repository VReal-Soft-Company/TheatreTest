using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestTheare.Shared.Data.Exceptions;
using TestTheatre.DAL.Context;
using TestTheatre.Shared.Data;

namespace TestTheatre.BLL.Services.Base
{
    public interface IBaseService<T> where T : BaseEntity
    {
        void PartialUpdate(T entity, Expression<Func<T, object>> property);
        Task<T> CreateAsync(T model);
        Task CreateRangeAsync(List<T> models);
        Task<T> GetById(long id);
        IQueryable<T> GetAll();
        Task<bool> DeleteAsync(long id);
        Task<T> Update(T model, long? id = null);
        Task IsExist(long id);
        bool Any(Func<T, bool> predicateForAny);
        Task<List<T>> GetAllBasicEntityAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicateForAny = null);
        Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicateForAny);
        Task<bool> AnyAsync();
        T FirstOrDefault(Func<T, bool> predicateForFirstOrDefault);
        Task<T> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicateForFirstOrDefault);

        IEnumerable<T> Get(Func<T, bool> predicate);
    }

    public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected ApplicationDataContext _context;
        public BaseService(ApplicationDataContext contex)
        {
            _context = contex;
        }
        public virtual async Task<T> CreateAsync(T model)
        {
            var entity = _context.Set<T>().Add(model);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public virtual async Task CreateRangeAsync(List<T> models)
        {
            await _context.Set<T>().AddRangeAsync(models);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(long id)
        {

            var entry = await GetById(id);
            entry.IsDeleted = true;
            var result = _context.Set<T>().Update(entry);
            await _context.SaveChangesAsync();
            return result.Entity.IsDeleted;

        }
        public bool Any(Func<T, bool> fileDescriptionPredicate)
        {
            return _context.Set<T>().Any(fileDescriptionPredicate);
        }
        public async Task<bool> AnyAsync(System.Linq.Expressions.Expression<Func<T, bool>> fileDescriptionPredicate)
        {
            return await _context.Set<T>().AnyAsync(fileDescriptionPredicate);
        }
        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().Where(x => !x.IsDeleted);
        }
        public virtual void PartialUpdate(T entity, Expression<Func<T, object>> property)
        {
            var entry = _context.Entry(entity);
            _context.Set<T>().Attach(entity);
            entry.Property(property).IsModified = true;
            _context.SaveChanges();
        }


        public virtual async Task<T> GetById(long id)
        {
            var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                throw new AppException($"{typeof(T).Name} with this Id not found or deleted", 404);
            return entity;
        }

        public async Task IsExist(long id)
        {
            var entity = await _context.Set<T>().AsNoTracking().AnyAsync(x => x.Id == id && !x.IsDeleted);
            if (!entity)
                throw new AppException($"{typeof(T).Name} with this Id not found or deleted", 404);
        }

        public virtual async Task<T> Update(T model, long? id = null)
        {
            if (id != null)
            {
                await IsExist(id.Value);
                model.Id = id.Value;
            }
            var entry = _context.Set<T>().Update(model);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public virtual T FirstOrDefault(Func<T, bool> predicateForFirstOrDefault)
        {
            return GetAll().FirstOrDefault(predicateForFirstOrDefault);
        }

        public async Task<T> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicateForFirstOrDefault)
        {
            return await GetAll().FirstOrDefaultAsync(predicateForFirstOrDefault);
        }

        public async Task<List<T>> GetAllBasicEntityAsync(Expression<Func<T, bool>> predicateForAny = null)
        {
            var query = _context.Set<T>().Where(f => !f.IsDeleted);
            if (predicateForAny != null)
            {
                query = query.Where(predicateForAny);
            }
            return await query.ToListAsync();
        }

        public async Task<bool> AnyAsync()
        {
            return await _context.Set<T>().AnyAsync();
        }
    }
}
