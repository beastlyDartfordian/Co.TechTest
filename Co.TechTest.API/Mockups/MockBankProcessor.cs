using System;

using System.Threading;
using System.Threading.Tasks;

namespace Co.TechTest.API
{
    public class MockBankProcessor : IBankProcessor
    {
        public async Task<Payment> ProcessPaymentAsync (Payment payment)
        {
            Thread.Sleep(5000);

            if ((payment.PaymentDetails.Scheme == CardScheme.AmericanExpress && payment.PaymentDetails.CVV.Length != 4)
                || (payment.PaymentDetails.Scheme != CardScheme.AmericanExpress && payment.PaymentDetails.CVV.Length != 3))
            {
                BankResponse bankResponse = new BankResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Success = false
                };

                payment.BankTransactionReference = bankResponse.Id;

                if (bankResponse.Success) { payment.Status = Status.Paid; }

                return payment;
            }
            else
            {
                BankResponse bankResponse = new BankResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Success = true
                };

                payment.BankTransactionReference = bankResponse.Id;

                if (bankResponse.Success) { payment.Status = Status.Paid; }

                return payment;
            }

            
        }
    }
}
