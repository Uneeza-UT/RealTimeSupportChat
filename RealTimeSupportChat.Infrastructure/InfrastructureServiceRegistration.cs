using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealTimeSupportChat.Application.Contracts.Email;
using RealTimeSupportChat.Application.Contracts.Services;
using RealTimeSupportChat.Application.Models.Email;
using RealTimeSupportChat.Infrastructure.EmailService;
using RealTimeSupportChat.Infrastructure.Services;
using Resend;

namespace RealTimeSupportChat.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.AddHttpClient<ResendClient>();
            services.Configure<ResendClientOptions>(options =>
            {
                options.ApiToken = configuration.GetValue<string>("EmailSettings:ApiKey");
            });

            services.AddTransient<IResend, ResendClient>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            return services;
        }
    }
}
