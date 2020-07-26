using System.Threading.Tasks;

namespace Co.TechTest.API
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentAsync(string merchantId, string merchantTransactionReference);
        Task<Payment> AddPaymentAsync(Payment payment);
    }
}
