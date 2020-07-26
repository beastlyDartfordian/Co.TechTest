using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Co.TechTest.API
{
    public class MerchantRepository : IMerchantRepository
    {

        private ILogger<MerchantRepository> _logger;
        private readonly AppDbContext context;
        public MerchantRepository(ILogger<MerchantRepository> logger, AppDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<Merchant> GetMerchantAsync(string id)
        {
            _logger.LogInformation($"Attempting to get Merchant with ID: {id}");
            return context.Merchants.Find(id);
        }
    }
}
