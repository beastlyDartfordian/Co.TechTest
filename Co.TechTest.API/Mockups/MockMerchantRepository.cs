using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Co.TechTest.API
{
    public class MockMerchantRepository : IMerchantRepository
    {
        private List<Merchant> _merchants;

        public MockMerchantRepository()
        {
            _merchants = new List<Merchant>
            {
                new Merchant{Id = "bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6", Name = "Adidas", Payments = new List<Payment>()},
                new Merchant{Id = "0474907e-0680-494c-a5e0-2a5a140272c5", Name = "EasyJet", Payments = new List<Payment>()},
                new Merchant{Id = "d9d68239-d7db-4898-8fc5-de2634077a6b", Name = "Samsung", Payments = new List<Payment>()}
            };
        }

        public async Task<Merchant> GetMerchantAsync(string id)
        {
            Merchant merchant = _merchants.FirstOrDefault(m => m.Id == id);

            return merchant;
        }
    }
}
