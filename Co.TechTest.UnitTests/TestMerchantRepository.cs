using Co.TechTest.API;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Co.TechTest.UnitTests
{
    class TestMerchantRepository
    {
        private IMerchantRepository _merchantRepository;

        [Test]
        public async Task ShouldPassGetPayment()
        {
            _merchantRepository = new MockMerchantRepository();

            var result = await _merchantRepository.GetMerchantAsync("bd2347d5-98b3-4d4e-8773-4eb4b33ca0d6");

            Assert.IsTrue(result.Name == "Adidas");
        }
    }
}
