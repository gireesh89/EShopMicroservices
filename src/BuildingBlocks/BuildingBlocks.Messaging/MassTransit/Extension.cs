using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extension
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services,
            IConfiguration configuration,
            Assembly? assembly=null)
        {
            services.AddMassTransit(config =>
            {
                if (assembly != null)
                    config.AddConsumers(assembly);
                config.UsingRabbitMq((context, configurtor) =>
                {
                    configurtor.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:UserName"]);
                        host.Password(configuration["MessageBroker:Password"]);
                    });
                    configurtor.ConfigureEndpoints(context);
                });

            });
            return services;
        }
    }
}
