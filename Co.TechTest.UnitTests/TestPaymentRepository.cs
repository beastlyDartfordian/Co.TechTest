using Co.TechTest.API;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Co.TechTest.UnitTests
{
    public class TestPaymentRepository
    {
        private IPaymentRepository _paymentRepository;

        [Test]
        public async Task ShouldPassGetPayment()
        {
            _paymentRepository = new MockPaymentRepository();

            var result = await _paymentRepository.GetPaymentAsync("bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6", "TestReference");

            Assert.IsTrue(result.PaymentDetails.Name == "Test Name");
        }

        [Test]
        public async Task ShouldPassAddPayment()
        {
            _paymentRepository = new MockPaymentRepository();

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

            var result = await _paymentRepository.AddPaymentAsync(payment);

            Assert.IsTrue(result.PaymentDetails.Name == "Test Name");
        }
    }
}