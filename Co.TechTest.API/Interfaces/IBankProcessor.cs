using System.Threading.Tasks;

namespace Co.TechTest.API
{
    public interface IBankProcessor
    {
        Task<Payment> ProcessPaymentAsync(Payment payment);
    }
}
