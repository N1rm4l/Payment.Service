using Payment.Service.Models;

namespace Payment.Service.ISvcs
{
    public interface IRazorPay
    {
        Task<Razorpay.Api.Order> ProcessPayment(PaymentRequest obj);
        Task<Razorpay.Api.Payment> CompletePayment(ConfirmRequest obj);
    }
}
