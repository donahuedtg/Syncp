using SyncpWallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncpWallet.Services
{
    public interface IWalletService
    {
        Task<WalletDetailsModel> CreateWalletAsync(WalletCreateModel model);
    }
}
