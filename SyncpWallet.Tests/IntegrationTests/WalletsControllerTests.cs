using NUnit.Framework;
using SyncpWallet.Controllers;
using SyncpWallet.Models;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace SyncpWallet.Tests.IntegrationTests
{
    [TestFixture]
    public class WalletsControllerTests
    {
        [Test]
        public async Task Post_WithValidRequest_ShouldReturn200()
        {
            var controller = new WalletsController();
            var request = new WalletCreateModel
            {
                Name = "My Wallet",
                Currency = "BGN",
                Amount = 100,
            };

            var response = await controller.Post(request);
            var contentResult = response as OkNegotiatedContentResult<WalletDetailsModel>;
            Assert.That(response, Is.Not.Null);
            Assert.That(contentResult, Is.Not.Null);
            Assert.That(contentResult.Content.Name, Is.EqualTo(request.Name));
            Assert.That(contentResult.Content.Currency, Is.EqualTo(request.Currency));
            Assert.That(contentResult.Content.Amount, Is.EqualTo(request.Amount));
        }
    }
}
