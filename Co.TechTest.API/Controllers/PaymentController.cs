using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Co.TechTest.API
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]  
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        
        private readonly IBankProcessor _bankProcessor;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IPaymentRepository _paymentRepository;
        
        public PaymentController(ILogger<PaymentController> logger, IBankProcessor bankProcessor, IMerchantRepository merchantRepository, IPaymentRepository paymentRepository)
        {
            _logger = logger;
            _bankProcessor = bankProcessor;
            _merchantRepository = merchantRepository;
            _paymentRepository = paymentRepository;
        }
        
        // GET: Payment/
        [HttpGet("{merchantId}+{reference}", Name = "Get")]
        public async Task<IActionResult> Get(string merchantId, string reference)
        {
            try
            {
                _logger.LogInformation($"Get Payment Request - Merchant ID: {merchantId} + Payment Ref: {reference}");

                if (string.IsNullOrWhiteSpace(merchantId) || string.IsNullOrWhiteSpace(reference))
                {
                    return new BadRequestObjectResult("Merchant ID and Merchant Payment Reference Required");
                }

                Payment payment = await _paymentRepository.GetPaymentAsync(merchantId, reference);

                if (payment == null)
                {
                    _logger.LogInformation("Payment not found");
                    
                    return new BadRequestObjectResult("No Such Payment Found");
                }

                _logger.LogInformation("Payment found");

                return new OkObjectResult(payment);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());

                return new StatusCodeResult(500);
            }
        }

        
        //Assumption made here that even if it fails at the acuiring bank we want to store it. 
        //In future would probably want to return reason from bankl to the merchant.
        // POST: Payment
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Payment payment)
        {
            try
            {
                _logger.LogInformation($"Post Payment Request - Merchant ID: {payment.MerchantId} + Payment Ref: {payment.MerchantTransactionReference}");

                Merchant merchant = await _merchantRepository.GetMerchantAsync(payment.MerchantId);

                if (merchant == null)
                {
                    _logger.LogWarning($"Merchant ({payment.MerchantId}) not found");
                    return new BadRequestObjectResult("No Such Merchant Found");
                }

                payment.MerchantId = merchant.Id;

                _logger.LogInformation("Attempting to connect to bank");

                var bankResponse = await _bankProcessor.ProcessPaymentAsync(payment);

                _logger.LogInformation("Bank process complete");

                Payment storedPayment = await _paymentRepository.AddPaymentAsync(bankResponse);

                _logger.LogInformation("Payment Processed");

                return new OkObjectResult(payment);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());

                return new StatusCodeResult(500);
            }
        }
    }
}
