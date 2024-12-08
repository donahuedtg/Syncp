namespace SyncpWallet.Models.Wallets.Request
{
    using System.ComponentModel.DataAnnotations;

    public class OperationWalletRequestModel
    {
        [Required]
        public decimal? Amount { get; set; } = null;
    }
}