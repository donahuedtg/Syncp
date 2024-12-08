using SyncpWallet.Models;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SyncpWallet.Services
{
    public class WalletService : IWalletService
    {
        public async Task<WalletDetailsModel> CreateWalletAsync(WalletCreateModel model)
        {
            AssertModelIsValid(model);

            using (var conn = new DatabaseConnector())
            {
                using (var reader = await conn.ExecReaderAsyncTask("usp_WalletInsert", null,
                            new SqlParameter("@name", model.Name),
                            new SqlParameter("@amount", model.Amount),
                            new SqlParameter("@currency", model.Currency)))
                {
                    if (!reader.Read())
                    {
                        throw new Exception("Inserting new Wallet failed");
                    }
                    var id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WalletId")));
                    return new WalletDetailsModel { Id = id, Name = model.Name, Amount = model.Amount, Currency = model.Currency };
                }
            }
        }

        private void AssertModelIsValid(WalletCreateModel model)
        {
            if (model == null)
            {
                throw new ArgumentException($"Cannot insert null Wallet");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentException("Cannot insert Wallet without name");
            }
            if (model.Amount < 0)
            {
                throw new ArgumentException("Cannot insert Wallet with negative balance");
            }
            if (string.IsNullOrEmpty(model.Currency))
            {
                throw new ArgumentException("Cannot insert Wallet without currency");
            }
        }
    }
}