namespace SyncpWallet.Tests.IntegrationTests
{
    using System.Linq;
    using System.Web.Http;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using System.Collections.Generic;

    using FakeItEasy;
    using NUnit.Framework;

    using Controllers;
    using Services;
    using Models.Wallets.Database;
    using Models.Wallets.Request;

    [TestFixture]
    public class WalletsControllerTests
    {
        private readonly IWalletService walletService;

        public WalletsControllerTests()
            => this.walletService = A.Fake<IWalletService>();
        

        [Test]
        public async Task Post_WithValidRequest_ShouldReturn200()
        {
            // Arrange
            WalletsController controller = new WalletsController(this.walletService);

            // Act
            IHttpActionResult result = await controller.PostAsync(A.Fake<CreateWalletRequestModel>());

            // Assert
            var contentResult = result as OkNegotiatedContentResult<WalletDatabaseModel>;
            Assert.That(result, Is.Not.Null);
            Assert.That(contentResult, Is.Not.Null);
            this.AssertHttpMethod<WalletsController, HttpPostAttribute>(nameof(controller.PostAsync));
        }

        [Test]
        public async Task Get_ShouldReturn200()
        {
            // Arrange
            WalletsController controller = new WalletsController(this.walletService);

            // Act
            IHttpActionResult result = await controller.GetAsync();

            // Assert
            var contentResult = result as OkNegotiatedContentResult<IEnumerable<WalletDatabaseModel>>;
            Assert.That(result, Is.Not.Null);
            Assert.That(contentResult, Is.Not.Null);
            this.AssertHttpMethod<WalletsController, HttpGetAttribute>(nameof(controller.GetAsync));
        }

        [Test]
        public async Task Delete_ShouldReturn200()
        {
            // Arrange
            WalletsController controller = new WalletsController(this.walletService);

            // Act
            IHttpActionResult result = await controller.DeleteAsync(1);

            // Assert
            var contentResult = result as OkNegotiatedContentResult<int>;
            Assert.That(result, Is.Not.Null);
            Assert.That(contentResult, Is.Not.Null);
            this.AssertHttpMethod<WalletsController, HttpDeleteAttribute>(nameof(controller.DeleteAsync));
        }

        [Test]
        public async Task Expense_ShouldReturn200()
        {
            // Arrange
            WalletsController controller = new WalletsController(this.walletService);

            // Act
            IHttpActionResult result = await controller.PutExpenseAsync(0, A.Fake<OperationWalletRequestModel>());

            // Assert
            var contentResult = result as OkNegotiatedContentResult<WalletOperationDatabaseModel>;
            Assert.That(result, Is.Not.Null);
            Assert.That(contentResult, Is.Not.Null);
            this.AssertHttpMethod<WalletsController, HttpPutAttribute>(nameof(controller.PutExpenseAsync));
        }

        [Test]
        public async Task Income_ShouldReturn200()
        {
            // Arrange
            WalletsController controller = new WalletsController(this.walletService);

            // Act
            IHttpActionResult result = await controller.PutIncomeAsync(0, A.Fake<OperationWalletRequestModel>());

            // Assert
            var contentResult = result as OkNegotiatedContentResult<WalletOperationDatabaseModel>;
            Assert.That(result, Is.Not.Null);
            Assert.That(contentResult, Is.Not.Null);
            this.AssertHttpMethod<WalletsController, HttpPutAttribute>(nameof(controller.PutIncomeAsync));
        }

        private void AssertHttpMethod<TController, THttpAttribute>(string actionName)
            where TController : ApiController
            where THttpAttribute : class
        {
            bool isHttpMethodValid = typeof(TController)
                .GetMethod(actionName)
                .GetCustomAttributes(true)
                .Any(atr => atr.GetType() == typeof(THttpAttribute));

            Assert.That(isHttpMethodValid, $"The actual method type is not match with {typeof(THttpAttribute).Name}");
        }
    }
}
