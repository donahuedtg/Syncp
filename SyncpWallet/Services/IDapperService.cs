namespace SyncpWallet.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IDapperService
    {
        Task<T> ExecuteFirstAsync<T>(string procedureName, object parameters = null);

        Task<IEnumerable<TModel>> ExecuteListAsync<TModel>(string procedureName, object parameters = null);
    }
}