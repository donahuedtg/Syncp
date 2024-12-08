namespace SyncpWallet.Models
{
    public class WalletDetailsModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Currency { get; set; }
        
        public double Amount { get; set; }
    }
}