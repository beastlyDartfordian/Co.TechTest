using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co.TechTest.API
{
    public class MockPaymentRepository : IPaymentRepository
    {

        private List<Payment> _payments;
        
        public MockPaymentRepository()
        {

            _payments = new List<Payment>
            {
                new Payment
                {
                    Id = 1,
                    MerchantId = "bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6",
                    Merchant = new Merchant{Id = "bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6", Name = "Adidas", Payments = new List<Payment>()},
                    MerchantTransactionReference = "TestReference",
                    BankTransactionReference = Guid.NewGuid().ToString(),
                    PaymentMethod = 0,
                    Amount = 22.99M,
                    Currency = Currency.GBP,
                    Status = Status.Paid,
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
                }
            };
        }
        
        public async Task<Payment> GetPaymentAsync(string merchantId, string merchantTransactionReference)
        {
            Payment payment = _payments.FirstOrDefault(p => p.MerchantId == merchantId && p.MerchantTransactionReference == merchantTransactionReference);

            return payment;
        }
        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            payment.PaymentDetails.CardNumber = payment.PaymentDetails.CardNumber.Substring(payment.PaymentDetails.CardNumber.Length - 4);
            payment.PaymentDetails.CVV = null;

            payment.Id = _payments.Count + 1;            
            _payments.Add(payment);

            return payment;
        }
    }
}
