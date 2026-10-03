using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealTimeSupportChat.Application.Contracts.Persistence;
using RealTimeSupportChat.Persistence.DatabaseContext;
using RealTimeSupportChat.Persistence.Repositories;

namespace RealTimeSupportChat.Persistence
{
    public static class PersistenceServiceRegistration
    {
        // Registers services and dependencies belonging to the Persistence layer.
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();    

            return services;
        }
    }
}
