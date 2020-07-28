using Account.Data;
using Account.Service;
using Account.Service.Intefaces;
using Account.Share.Interfaces;
using Account.WebApi.Miidleware;
using Account.WebApi.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Account.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOperationHistoryService, OperationHistoryService>();
            services.AddScoped<IOperationHistoryRepository, OperationHistoryRepository>();            
            services.AddDbContext<AccountContext>(options =>
                          options.UseSqlServer(
                              Configuration.GetConnectionString("FinalProject_Account")));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AccountProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //for pagination
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            //for pagination
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                       builder =>
                       {
                           builder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .WithExposedHeaders("X-Pagination");
                       });
            });

            services.AddControllers();
            services.AddMvc();
            services.AddAuthorization();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "BankOpenAPISpecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Bank API",
                        Version = "1"
                    });
            });
        }
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();           
            app.UseRouting();
            app.UseCors();
            app.UseMiddleware(typeof(AccountErrorHandlerMiddleware));
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    "/swagger/BankOpenAPISpecification/swagger.json",
                    "Bank API"
                    );
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
