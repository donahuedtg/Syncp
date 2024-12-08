namespace SyncpWallet.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Models.Wallets.Database;
    using Models.Wallets.Request;

    public interface IWalletService
    {
        Task<IEnumerable<WalletDatabaseModel>> GetAllWalletAsync();
        Task<WalletDatabaseModel> CreateWalletAsync(CreateWalletRequestModel request);
        Task<WalletOperationDatabaseModel> ExpenseWalletAsync(int id, OperationWalletRequestModel request);
        Task<WalletOperationDatabaseModel> IncomeWalletAsync(int id, OperationWalletRequestModel request);
        Task<int> DeleteWalletAsync(int id);
    }
}
