namespace Payment.Service.Models
{
    public class PaymentRequest
    {
        public string Receipt { get; set; }
        public decimal Amount { get; set; }
    }
}
