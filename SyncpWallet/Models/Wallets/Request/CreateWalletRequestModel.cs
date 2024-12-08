namespace SyncpWallet.Models.Wallets.Request
{
    using System.ComponentModel.DataAnnotations;

    public class CreateWalletRequestModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }


        [Required]
        [Range(0, double.MaxValue)]
        public double? Amount { get; set; } = null;


        [Required]
        [MinLength(3)]
        public string Currency { get; set; }
    }
}