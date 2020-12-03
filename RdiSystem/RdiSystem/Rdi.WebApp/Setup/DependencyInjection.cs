using Microsoft.Extensions.DependencyInjection;
using Rdi.Application.Services;
using Rdi.Data.OrderRepository;
using Rdi.Data.Repository;

namespace Rdi.WebApp.Setup
{
    public static class DependencyInjection
    {

        public static void RegisterServices(this IServiceCollection services)
        {
            
            // Services
            services.AddTransient<IOrderService, OrderService>();


            // Repository
            services.AddTransient<IKitchenAreasRepository, KitchenAreasRepository>();
            
        }
    }
}
