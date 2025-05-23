using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string table);
        Task<T?> GetByIdAsync(string table, object id);
        Task<int> InsertAsync(string table, T entity);
        Task<int> UpdateAsync(string table, T entity, object id);
        Task<int> DeleteAsync(string table, object id);
        Task<IEnumerable<T>> ExecuteStoredProcedureAsync(string procedureName, object? parameters = null);
        Task<int> ExecuteNonQueryProcedureAsync(string procedureName, object? parameters = null);
        Task<TOut> ExecuteWithOutputAsync<TOut>(string procedureName, Action<DynamicParameters> configureParams, string outputParamName);

    }
}
