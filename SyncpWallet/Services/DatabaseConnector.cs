using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SyncpWallet.Services
{
    public class DatabaseConnector : IDisposable
    {
        private const int DefaultDatabaseTimeout = 30;
        private string connectionString;
        public SqlConnection Connection { get; private set; }

        public DatabaseConnector()
        {
            connectionString = ConfigurationManager.AppSettings["DefaultConnection"];
            Connection = GetConnection();
        }

        public async Task<SqlDataReader> ExecReaderAsyncTask(string cmdName, SqlTransaction trans, params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = GetCommand(cmdName, parameters))
            {
                cmd.CommandTimeout = DefaultDatabaseTimeout;

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                return await cmd.ExecuteReaderAsync();
            }
        }

        public async Task<int> ExecNonQueryAsyncTask(string cmdName, SqlTransaction trans, params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = GetCommand(cmdName, parameters))
            {
                cmd.CommandTimeout = DefaultDatabaseTimeout;

                if (trans != null)
                {
                    cmd.Transaction = trans;
                }

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public SqlCommand GetCommand(string commandName, SqlParameter[] parameters)
        {
            var cmd = new SqlCommand(commandName, Connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd;
        }

        private SqlConnection GetConnection()
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();

            return conn;
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}