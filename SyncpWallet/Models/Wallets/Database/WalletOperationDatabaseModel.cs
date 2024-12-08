namespace SyncpWallet.Models.Wallets.Database
{
    using System;

    public class WalletOperationDatabaseModel
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal IncomeAmount { get; set; }
        public DateTime? TimeStamp { get; set; } = null;
	}
}