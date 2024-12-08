namespace SyncpWallet.Models.Wallets.Database
{
    public class WalletDatabaseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Currency { get; set; }

        public double Amount { get; set; }
    }
}