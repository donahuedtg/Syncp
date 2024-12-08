namespace SyncpWallet.Services
{
    using System.Data;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Dapper;

    public class DapperService : IDapperService
    {
        private readonly string sqlConnSting = ConfigurationManager.AppSettings["DefaultConnection"];

        public async Task<T> ExecuteFirstAsync<T>(string procedureName, object parameters = null)
        {
            using (IDbConnection connection = new SqlConnection(this.sqlConnSting))
            {
                return await connection.QueryFirstOrDefaultAsync<T>
                    (
                        procedureName,
                        parameters,
                        null,
                        120,
                        CommandType.StoredProcedure
                    );
            }
        }

        public async Task<IEnumerable<TModel>> ExecuteListAsync<TModel>(string procedureName, object parameters = null)
        {
            IEnumerable<TModel> list = null;

            using (IDbConnection connection = new SqlConnection(this.sqlConnSting))
            {
                list = await connection.QueryAsync<TModel>
                    (
                        procedureName,
                        parameters,
                        null,
                        120,
                        CommandType.StoredProcedure
                    );
            }

            return list;
        }
    }
}