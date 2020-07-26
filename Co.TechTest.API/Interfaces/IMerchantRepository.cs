using System.Threading.Tasks;

namespace Co.TechTest.API
{
    public interface IMerchantRepository
    {
        Task<Merchant> GetMerchantAsync(string id);
    }
}
