using Co.TechTest.API;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Co.TechTest.UnitTests
{
    class TestPaymentController
    {

        private IBankProcessor _bankProcessor;
        private IMerchantRepository _merchantRepository;
        private IPaymentRepository _paymentRepository;

        [SetUp]
        public void Setup()
        {
            _bankProcessor = new MockBankProcessor();
            _merchantRepository = new MockMerchantRepository();
            _paymentRepository = new MockPaymentRepository();
        }

        [Test]
        public async Task ShouldFailGet()
        {
            var sut = new PaymentController(new Mock<ILogger<PaymentController>>().Object, _bankProcessor, _merchantRepository, _paymentRepository);

            var response = await sut.Get(string.Empty, string.Empty);

            var responseType = response.GetType();

            Assert.IsTrue(responseType.Name == "BadRequestObjectResult");
        }

        [Test]
        public async Task ShouldPassGet()
        {

            var sut = new PaymentController(new Mock<ILogger<PaymentController>>().Object, _bankProcessor, _merchantRepository, _paymentRepository);

            var response = await sut.Get("bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6", "TestReference");

            var responseType = response.GetType();

            Assert.IsTrue(responseType.Name == "OkObjectResult");
        }
        
        [Test]
        public async Task ShouldFailPost_Validation()
        {
            var sut = new PaymentController(new Mock<ILogger<PaymentController>>().Object, _bankProcessor, _merchantRepository, _paymentRepository);

            var response = await sut.Post(new Payment());

            var responseType = response.GetType();
            
            Assert.IsTrue(responseType.Name == "BadRequestObjectResult");
  
        }

        [Test]
        public async Task ShouldPassPost()
        {

            var sut = new PaymentController(new Mock<ILogger<PaymentController>>().Object, _bankProcessor, _merchantRepository, _paymentRepository);

            var payment = new Payment
            {
                MerchantId = "bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6",
                MerchantTransactionReference = "TestReference2",
                PaymentMethod = 0,
                Amount = 22.99M,
                Currency = Currency.GBP,
                PaymentDetails = new PaymentDetails
                {
                    CardNumber = "1234567812345678",
                    Name = "Test Name",
                    ExpiryMonth = "12",
                    ExpiryYear = "2030",
                    CVV = "123",
                    Scheme = CardScheme.Mastercard,
                    Type = CardType.Credit,
                    Address = new Address
                    {
                        Number = 1,
                        Street = "Test Road",
                        City = "Test City",
                        County = "Test County",
                        PostCode = "TE5 7PC"
                    }
                }
            };

            var response = await sut.Post(payment);

            var responseType = response.GetType();

            Assert.IsTrue(responseType.Name == "OkObjectResult");
        }
    }
}
