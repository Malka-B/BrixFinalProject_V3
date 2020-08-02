using Messages.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using System.IO;

namespace Account.Api
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
     .Build();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
  .UseNServiceBus(context =>
  {
      var endpointConfiguration = new EndpointConfiguration("Account");
      endpointConfiguration.EnableInstallers();
      var outboxSettings = endpointConfiguration.EnableOutbox();
      var recoverability = endpointConfiguration.Recoverability();
      recoverability.Delayed(
          customizations: delayed =>
          {
              delayed.NumberOfRetries(2);
              delayed.TimeIncrease(TimeSpan.FromMinutes(4));
          });
      recoverability.Immediate(
          customizations: immediate =>
          {
              immediate.NumberOfRetries(3);

          });

      var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
      transport.UseConventionalRoutingTopology()
      .ConnectionString("host = localhost:5672; username = guest; password = guest");//Configuration.GetConnectionString("RabbitMQ"));
      var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
      var connection =Configuration.GetConnectionString("AccountOutbox");
      persistence.SqlDialect<SqlDialect.MsSqlServer>();
      persistence.ConnectionBuilder(
          connectionBuilder: () =>
          {
              return new SqlConnection(connection);
          });

      var routing = transport.Routing();
      routing.RouteToEndpoint(
           messageType: typeof(SendEmail),
           destination: "AccountNSB");
      endpointConfiguration.SendFailedMessagesTo("error");
      endpointConfiguration.AuditProcessedMessagesTo("audit");
      var conventions = endpointConfiguration.Conventions();
      conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
      conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");
      return endpointConfiguration;
  })
   .ConfigureWebHostDefaults(webBuilder =>
   {
       webBuilder.UseStartup<Startup>()
                 .UseConfiguration(Configuration);
   });
    }

}
