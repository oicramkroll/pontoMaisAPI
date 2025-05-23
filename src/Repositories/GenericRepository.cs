using Dapper;
using Services;
using System.Data;

namespace Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDatabaseConnection _db;

        public GenericRepository(IDatabaseConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string table)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<T>($"SELECT * FROM {table}");
        }

        public async Task<T?> GetByIdAsync(string table, object id)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {table} WHERE id = @id", new { id });
        }

        public async Task<int> InsertAsync(string table, T entity)
        {
            using var conn = _db.CreateConnection();
            var props = typeof(T).GetProperties().Where(p => p.Name.ToLower() != "id").ToList();

            var columns = string.Join(", ", props.Select(p => p.Name));
            var values = string.Join(", ", props.Select(p => "@" + p.Name));

            var sql = $"INSERT INTO {table} ({columns}) VALUES ({values})";

            return await conn.ExecuteAsync(sql, entity);
        }

        public async Task<int> UpdateAsync(string table, T entity, object id)
        {
            using var conn = _db.CreateConnection();
            var props = typeof(T).GetProperties().Where(p => p.Name.ToLower() != "id").ToList();

            var setClause = string.Join(", ", props.Select(p => $"{p.Name} = @{p.Name}"));

            var sql = $"UPDATE {table} SET {setClause} WHERE id = @id";

            var parameters = new DynamicParameters(entity);
            parameters.Add("id", id);

            return await conn.ExecuteAsync(sql, parameters);
        }

        public async Task<int> DeleteAsync(string table, object id)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync($"DELETE FROM {table} WHERE id = @id", new { id });
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync(string procedureName, object? parameters = null)
        {
            using var conn = _db.CreateConnection();
            return await conn.QueryAsync<T>(
                sql: procedureName,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<int> ExecuteNonQueryProcedureAsync(string procedureName, object? parameters = null)
        {
            using var conn = _db.CreateConnection();
            return await conn.ExecuteAsync(
                sql: procedureName,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        public async Task<TOut> ExecuteWithOutputAsync<TOut>(string procedureName, Action<DynamicParameters> configureParams, string outputParamName)
        {
            using var conn = _db.CreateConnection();

            var parameters = new DynamicParameters();
            configureParams(parameters);

            await conn.ExecuteAsync(
                sql: procedureName,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<TOut>(outputParamName);
        }


    }
}
