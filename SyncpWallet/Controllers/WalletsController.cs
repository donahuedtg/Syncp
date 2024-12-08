namespace SyncpWallet.Controllers
{
    using System.Web.Http;
    using System.Threading.Tasks;

    using Services;
    using Models.Wallets.Request;

    [RoutePrefix("api/Wallets")]
    public class WalletsController : ApiController
    {
        private readonly IWalletService walletService;

        public WalletsController(IWalletService walletService)
            => this.walletService = walletService;
        
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
            => Ok(await this.walletService.GetAllWalletAsync());

        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] CreateWalletRequestModel request)
            => Ok(await this.walletService.CreateWalletAsync(request));

        [HttpPut]
        [Route("Expense/{id:int}")]
        public async Task<IHttpActionResult> PutExpenseAsync(int id, [FromBody] OperationWalletRequestModel request)
            => Ok(await this.walletService.ExpenseWalletAsync(id, request));

        [HttpPut]
        [Route("Income/{id:int}")]
        public async Task<IHttpActionResult> PutIncomeAsync(int id, [FromBody] OperationWalletRequestModel request)
            => Ok(await this.walletService.IncomeWalletAsync(id, request));

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteAsync(int id)
            => Ok(await this.walletService.DeleteWalletAsync(id));
    }
}