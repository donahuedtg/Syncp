using SyncpWallet.Models;
using SyncpWallet.Services;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace SyncpWallet.Controllers
{
    public class WalletsController : ApiController
    {
        public async Task<IHttpActionResult> Post([FromBody] WalletCreateModel model)
        {
            try
            {
                var response = await new WalletService().CreateWalletAsync(model);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}