using Microsoft.Extensions.DependencyInjection;
using Payment.Service.ISvcs;
using Payment.Service.Svcs;

namespace Payment.Service
{
    public static class Startup
    {
        public static void PaymentService(this IServiceCollection services)
        {
            services.AddScoped<IRazorPay, RazorPay>();
        }
    }
}
