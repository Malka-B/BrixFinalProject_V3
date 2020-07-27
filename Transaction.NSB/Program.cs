using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Transaction.Data;
using Transaction.Share.Interfaces;
using Messages.Commands;
using NServiceBus.Transport;
using System;
using AutoMapper;
using Tracking.WebApi.Profiles;

namespace Transaction.NSB
{
    class Program
    {
        static async Task Main()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            //Console.Title = GetQueue("queueName");
            Console.Title = "TransactionNSB";
            //var endpointConfiguration = new EndpointConfiguration(GetQueue("queueName"));            
            var endpointConfiguration = new EndpointConfiguration("TransactionNSB");
            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());           
            containerSettings.ServiceCollection.AddScoped<ITransactionRepository, TransactionRepository>();
            containerSettings.ServiceCollection.AddDbContext<TransactionContext>(
           //options => options.UseSqlServer(configuration.GetConnectionString("FinalProject_Transaction")));
           options => options.UseSqlServer("Server = C1; Database = BrixFinalProject_Transaction ;Trusted_Connection=True;"));
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TransactionProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            containerSettings.ServiceCollection.AddSingleton(mapper);
            endpointConfiguration.EnableOutbox();
            endpointConfiguration.EnableInstallers();
            //var connection = configuration.GetConnectionString("TransactionOutbox");
            var connection = "Server = C1; Database = TransactionNSBOutbox ;Trusted_Connection=True;";
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });


            var subscription = persistence.SubscriptionSettings();
            subscription.CacheFor(TimeSpan.FromMinutes(1));
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            //transport.ConnectionString(configuration.GetConnectionString("RabbitMQ"));
            transport.ConnectionString("host= localhost:5672;username=guest;password=guest");

            var routing = transport.Routing();
            routing.RouteToEndpoint(
                 messageType: typeof(UpdateAccounts),
                 destination: "AccountNSB");
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(
                immediate =>
                {
                    immediate.NumberOfRetries(3);
                });
            recoverability.Delayed(
                delayed =>
                {
                    var retries = delayed.NumberOfRetries(3);
                    retries.TimeIncrease(TimeSpan.FromSeconds(2));
                });
            //endpointConfiguration.SendFailedMessagesTo(GetQueue("errorQueue"));
            //endpointConfiguration.AuditProcessedMessagesTo(GetQueue("auditQueue"));
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
            string GetQueue(string queueName)
            {
                return configuration.GetSection("Queues").GetSection(queueName).Value;
            }
        }
    }
}