namespace Payment.Service.Models
{
    public class ConfirmRequest
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string Signature { get; set; }
    }
}
