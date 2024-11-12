using AdminPanel.Shared;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll(string includePath = "");

        Task<TEntity> Get(int id);

        Task<TEntity> Get(long id);

        Task<TEntity> GetSingleWithConditions(Expression<Func<TEntity, bool>> expression, string includePath = "");

        IQueryable<TEntity> GetWithConditions(Expression<Func<TEntity, bool>> expression, string includePath = "", bool asNoTracking = false);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<TEntity> Delete(long id);
        
        Task<TEntity> Delete(int id);
        
        Task Delete(TEntity entity);

        Task DeleteRange(IEnumerable<TEntity> range);

        Task BulkInsert(List<TEntity> entity);

        Task BulkDelete(ImportFor importType, IList<TEntity> olditems);

        Task UpdateBitField(object id, string colName, bool value, long modifiedBy);
    }
}
