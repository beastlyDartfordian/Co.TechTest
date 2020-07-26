using Co.TechTest.API;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Co.TechTest.UnitTests
{
    class TestBankProcessor
    {
        private IBankProcessor _bankProcessor;

        [Test]
        public async Task ShouldPassProcessPayment()
        {
            _bankProcessor = new MockBankProcessor();

            var payment = new Payment
            {
                MerchantId = "bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6",
                MerchantTransactionReference = "TestReference",
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


            var result = await _bankProcessor.ProcessPaymentAsync(payment);

            Assert.IsTrue(result.Status == Status.Paid);
        }

        [Test]
        public async Task ShouldFailProcessPayment()
        {
            _bankProcessor = new MockBankProcessor();

            var payment = new Payment
            {
                MerchantId = "bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6",
                MerchantTransactionReference = "TestReference",
                PaymentMethod = 0,
                Amount = 22.99M,
                Currency = Currency.GBP,
                PaymentDetails = new PaymentDetails
                {
                    CardNumber = "1234567812345678",
                    Name = "Test Name",
                    ExpiryMonth = "12",
                    ExpiryYear = "2030",
                    CVV = "1234",
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


            var result = await _bankProcessor.ProcessPaymentAsync(payment);

            Assert.IsTrue(result.Status == Status.Unpaid);
        }
    }
}
