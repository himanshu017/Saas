using AdminPanel.DataModel.Context;
using AdminPanel.Shared;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        protected readonly OrderflowContext _context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(OrderflowContext context)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Get all records for an entity
        /// </summary>
        /// <param name="includePath">Add table name for including that via Lazy loading in the call. Use comma seperated values to include multiple tables </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IQueryable<TEntity> GetAll(string includePath = "")
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (!string.IsNullOrEmpty(includePath))
                {
                    var tables = includePath.Split(',');
                    foreach (var item in tables)
                    {
                        query = query.Include(item);
                    }
                }
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message} : Inner Ex:: {ex.InnerException?.Message}");
            }
        }

        public async Task<TEntity> Get(int id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve the record: {ex.Message} :  Inner Ex:: {ex.InnerException?.Message}");
            }
        }

        public async Task<TEntity> Get(long id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve the record: {ex.Message} :  Inner Ex:: {ex.InnerException?.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message} :  Inner Ex:: {ex.InnerException?.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");
            }

            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message} :  Inner Ex:: {ex.InnerException?.Message}");
            }
        }

        public async Task<TEntity> Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException($"Id must not be null");
            }

            try
            {
                var entity = await dbSet.FindAsync(id);
                if (entity == null)
                {
                    throw new ArgumentNullException($"{nameof(TEntity)} must not be null");
                }

                dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Record could not be deleted: {ex.Message} : Inner Ex:: {ex.InnerException?.Message}");
            }
        }

        public async Task<TEntity> Delete(long id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException($"Id must not be null");
            }

            try
            {
                var entity = await dbSet.FindAsync(id);
                if (entity == null)
                {
                    throw new ArgumentNullException($"{nameof(TEntity)} must not be null");
                }

                dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"Record could not be deleted: {ex.Message} : Inner Ex:: {ex.InnerException?.Message}");
            }
        }

        public IQueryable<TEntity> GetWithConditions(Expression<Func<TEntity, bool>> expression, string includePath = "", bool asNoTracking = false)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (!string.IsNullOrEmpty(includePath))
                {
                    var tables = includePath.Split(',');
                    foreach (var item in tables)
                    {
                        query = query.Include(item);
                    }
                }
                if (asNoTracking)
                {
                    return query.AsNoTracking().Where(expression);
                }
                return query.Where(expression);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve the records: {ex.Message} : Inner Ex:: {ex.InnerException?.Message}");
            }
        }

        public async Task<TEntity> GetSingleWithConditions(Expression<Func<TEntity, bool>> expression, string includePath = "")
        {
            try
            {
                IQueryable<TEntity> query = dbSet;
                if (!string.IsNullOrEmpty(includePath))
                {
                    var tables = includePath.Split(',');
                    foreach (var item in tables)
                    {
                        query = query.Include(item);
                    }
                }

                var res = await query.Where(expression).FirstOrDefaultAsync();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve the record: {ex.Message} :  Inner Ex:: {ex.InnerException?.Message}");
            }
        }

        public async Task DeleteRange(IEnumerable<TEntity> range)
        {
            try
            {
                dbSet.RemoveRange(range);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete records for {nameof(TEntity)}: {ex.Message} Inner Ex: {ex.InnerException?.Message}");
            }
        }

        public async Task Delete(TEntity entity)
        {
            try
            {
                dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't delete records for {nameof(TEntity)}: {ex.Message} Inner Ex: {ex.InnerException?.Message}");
            }
        }

        public async Task BulkInsert(List<TEntity> entity)
        {
            try
            {

                await _context.BulkInsertAsync(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't insert records for {nameof(TEntity)}: {ex.Message} Inner Ex: {ex.InnerException?.Message}");
            }
        }

        public async Task BulkDelete(ImportFor importType, IList<TEntity> olditems)
        {
            try
            {
                if (importType == ImportFor.Product)
                    await _context.BulkDeleteAsync(olditems);
                else if (importType == ImportFor.Customer)
                    await _context.BulkDeleteAsync(olditems);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task UpdateBitField(object id, string colName, bool newValue, long modifiedBy)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity != null)
            {
                entity.GetType().GetProperty(colName).SetValue(entity, newValue, null);

                var prop = entity.GetType().GetProperty("ModifiedBy");
                if (prop != null)
                {
                    prop.SetValue(entity, modifiedBy, null);
                }

                prop = entity.GetType().GetProperty("ModifiedOn");
                
                if (prop != null)
                {
                    prop.SetValue(entity, DateTime.UtcNow, null);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
