using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Co.TechTest.API
{
    public class Payment 
    {
        [Key]
        public int Id { get; set; }
        [Required()]
        public string MerchantTransactionReference { get; set; }
        [Required()]
        public string MerchantId { get; set; }
        public Merchant Merchant { get; set; }
        public string BankTransactionReference { get; set; }
        public Status Status { get; set; }
        [Required()]
        public PaymentMethod PaymentMethod { get; set; }
        [Required()]
        public int PaymentDetailsId { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
        [Required()]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }
        [Required()]
        public Currency Currency { get; set; }
        
    }

    public class PaymentDetails //For Brevity focused on card details
    {
        [Key]
        public int Id { get; set; }
        [Required()]
        public string CardNumber { get; set; }
        public string Name { get; set; }
        [MinLength(2), MaxLength(2)]
        public string StartMonth { get; set; }
        [MinLength(4), MaxLength(4)]
        public string StartYear { get; set; }
        [Required(), MinLength(2), MaxLength(2)]
        public string ExpiryMonth { get; set; }
        [Required(), MinLength(4), MaxLength(4)]
        public string ExpiryYear { get; set; }
        [Required, MinLength(3), MaxLength(4)]
        public string CVV { get; set; }
        public CardScheme Scheme { get; set; }
        public CardType Type { get; set; }
        public int AddressId {get;set;}
        public Address Address { get; set; } //Is whole address needed?
        public Payment Payment { get; set; }
    }


    public class Address //Based on personal knowledge of Britsh Postal Addresses. Would need reworking for other countries
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
 
        public PaymentDetails PaymentDetails { get; set; }
    }

    // These enums may be better of in a data structure for easy addition / removal?

    public enum CardScheme //Based on my rough knowledge of UK/US. Others probably available
    {
        Visa = 0,
        Mastercard = 1,
        AmericanExpress = 2,
        Discover = 3,
        DinersClub = 4,
        JCB = 5,
        UnionPay = 6
    }

    public enum CardType //Could be extended to include other types. Is prepaid different?
    {
        Debit = 0,
        Credit = 1,
    }

    public enum Currency
    {
        GBP = 0,
        USD = 1,
        EUR = 2
    }

    public enum PaymentMethod //Only plan on using card for this example, Apple & Google pay could be added?
    {
        Card = 0,
    }

    public enum Status //Potential to add additional status types if required
    {
        Unpaid = 0,
        Paid = 1
    }

    
}
