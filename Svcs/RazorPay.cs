using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Payment.Service.ISvcs;
using Payment.Service.Models;
using Razorpay.Api;

namespace Payment.Service.Svcs
{
    public class RazorPay : IRazorPay
    {
        private readonly ILogger<RazorPay> _log;
        private readonly RazorpayClient _razorpayClient;
        private readonly IConfiguration _config;

        public RazorPay(ILogger<RazorPay> log, IConfiguration config)
        {
            _log = log;
            _config = config;
            _razorpayClient = new RazorpayClient(_config["RazorPay:Key"], _config["RazorPay:Secret"]);
        }

        public Task<Razorpay.Api.Order> ProcessPayment(PaymentRequest obj)
        {
            try
            {
                Dictionary<string, dynamic> options = new()
                {
                    { "amount", obj.Amount * 100 },
                    { "receipt", obj.Receipt },
                    { "currency", "INR" },
                    { "payment_capture", "0" }
                };

                Razorpay.Api.Order order = _razorpayClient.Order.Create(options);
                return Task.FromResult(order);
            }
            catch (Exception ex)
            {
                _log.LogError("Something went wrong: {ex}", ex);
                throw;
            }
        }

        public Task<Razorpay.Api.Payment> CompletePayment(ConfirmRequest obj)
        {
            try
            {
                Dictionary<string, string> options = new()
                {
                    { "razorpay_payment_id", obj.PaymentId },
                    { "razorpay_order_id", obj.OrderId },
                    { "razorpay_signature", obj.Signature }
                };

                var isValid = Utils.ValidatePaymentSignature(options);

                if (isValid)
                {
                    var order = _razorpayClient.Order.Fetch(obj.OrderId);
                    var payment = _razorpayClient.Payment.Fetch(obj.PaymentId);

                    Dictionary<string, dynamic> response = new()
                    {
                        { "amount", payment["amount"] }
                    };

                    Razorpay.Api.Payment paymentCaptured = payment.Capture(response);
                    return Task.FromResult(paymentCaptured);
                }

                throw new Exception("Invalid payment signature.");
            }
            catch (Exception ex)
            {
                _log.LogError("Something went wrong: {ex}", ex);
                throw;
            }
        }
    }
}
