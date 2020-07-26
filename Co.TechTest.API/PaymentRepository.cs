using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Co.TechTest.API
{
    public class PaymentRepository : IPaymentRepository
    {
        private ILogger<PaymentRepository> _logger;
        private readonly AppDbContext context;
        public PaymentRepository(ILogger<PaymentRepository> logger, AppDbContext context)
        {
            _logger = logger;
            this.context = context;
        }
        public async Task<Payment> GetPaymentAsync(string merchantId, string merchantTransactionReference)
        {
            _logger.LogInformation($"Attempting to get Payment with Merchant ID: {merchantId} + MTID: {merchantTransactionReference}");

            return context.Payments
                .Include(p => p.PaymentDetails)
                    .ThenInclude(pd => pd.Address)
                .FirstOrDefault(p => p.MerchantId == merchantId && p.MerchantTransactionReference == merchantTransactionReference);
        }
        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            _logger.LogInformation($"Attempting to save payment with Merchant ID: {payment.MerchantId} + MTID: {payment.MerchantTransactionReference}");

            payment.PaymentDetails.CardNumber = payment.PaymentDetails.CardNumber.Substring(payment.PaymentDetails.CardNumber.Length - 4);
            payment.PaymentDetails.CVV = String.Empty;

            context.Payments.Add(payment);
            context.SaveChanges();

            _logger.LogInformation("Payment Saved");

            return payment;
        }
    }
}
