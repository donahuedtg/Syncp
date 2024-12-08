namespace SyncpWallet.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Models.Wallets.Request;
    using Models.Wallets.Database;

    public class WalletService : IWalletService
    {
        private const string StoreProcedureGetWallets = "[dbo].[usp_WalletGet]";
        private const string StoreProcedureWalletInsert = "[dbo].[usp_WalletInsert]";
        private const string StoreProcedureWalletDelete = "[dbo].[usp_WalletDelete]";

        private const string StoreProcedureWalletExpenseOperation = "[dbo].[usp_WalletExpenseOperation]";
        private const string StoreProcedureWalletIncomeOperation = "[dbo].[usp_WalletIncomeOperation]";

        private readonly IDapperService dapperService;

        public WalletService(IDapperService executeDatabaseService)
            => this.dapperService = executeDatabaseService;


        //public async Task<WalletDetailsModel> CreateWalletAsync(WalletCreateModel model)
        //{
        //    AssertModelIsValid(model);

        //    using (var conn = new DatabaseConnector())
        //    {
        //        using (var reader = await conn.ExecReaderAsyncTask("usp_WalletInsert", null,
        //                    new SqlParameter("@name", model.Name),
        //                    new SqlParameter("@amount", model.Amount),
        //                    new SqlParameter("@currency", model.Currency)))
        //        {
        //            if (!reader.Read())
        //            {
        //                throw new Exception("Inserting new Wallet failed");
        //            }
        //            var id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WalletId")));
        //            return new WalletDetailsModel { Id = id, Name = model.Name, Amount = model.Amount, Currency = model.Currency };
        //        }
        //    }
        //}

        public async Task<IEnumerable<WalletDatabaseModel>> GetAllWalletAsync()
            => await this.dapperService.ExecuteListAsync<WalletDatabaseModel>(StoreProcedureGetWallets);


        public async Task<WalletDatabaseModel> CreateWalletAsync(CreateWalletRequestModel request)
            => await this.dapperService.ExecuteFirstAsync<WalletDatabaseModel>(
                    StoreProcedureWalletInsert,
                    new
                    {
                        @Name = request.Name,
                        @Amount = request.Amount,
                        @Currency = request.Currency
                    });

        public async Task<int> DeleteWalletAsync(int id)
            => await this.dapperService.ExecuteFirstAsync<int>(
                    StoreProcedureWalletDelete,
                    new
                    {
                        @Id = id
                    });

        public async Task<WalletOperationDatabaseModel> ExpenseWalletAsync(int id, OperationWalletRequestModel request)
            => await this.dapperService.ExecuteFirstAsync<WalletOperationDatabaseModel>(
                    StoreProcedureWalletExpenseOperation,
                    new
                    {
                        @WalletId = id,
                        @ExpenseAmount = request.Amount
                    });


        public async Task<WalletOperationDatabaseModel> IncomeWalletAsync(int id, OperationWalletRequestModel request)
            => await this.dapperService.ExecuteFirstAsync<WalletOperationDatabaseModel>(
                    StoreProcedureWalletIncomeOperation,
                    new
                    {
                        @WalletId = id,
                        @IncomeAmount = request.Amount
                    });


        //private void AssertModelIsValid(WalletCreateModel model)
        //{
        //    if (model == null)
        //    {
        //        throw new ArgumentException($"Cannot insert null Wallet");
        //    }
        //    if (string.IsNullOrEmpty(model.Name))
        //    {
        //        throw new ArgumentException("Cannot insert Wallet without name");
        //    }
        //    if (model.Amount < 0)
        //    {
        //        throw new ArgumentException("Cannot insert Wallet with negative balance");
        //    }
        //    if (string.IsNullOrEmpty(model.Currency))
        //    {
        //        throw new ArgumentException("Cannot insert Wallet without currency");
        //    }
        //}
    }
}