using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderApi.Consts;
using OrderApi.Courier.Activities;
using OrderApi.Infrastructure;
using OrderApi.Integrations.Consumers;
using OrderApi.Saga.SubmitOrderSagas;
using OrderApi.Saga.SubmitOrderTransactionalSaga;
using OrderApi.Services;
using Shared.Contract.Messages;
using System.Reflection;

namespace OrderApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static MassTransitConfig MassTransitConfig { get; set; }
        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var massTransitSettingSection = Configuration.GetSection("MassTransitConfig");
            var massTransitConfig = massTransitSettingSection.Get<MassTransitConfig>();

            services.AddDbContext<OrderDbContext>(options =>
                     options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddMassTransit(x =>
            {              
                x.AddConsumers(Assembly.GetExecutingAssembly());
                x.AddActivities(Assembly.GetExecutingAssembly());
                x.AddRequestClient<TakeProductTransactionMessage>();
                x.SetKebabCaseEndpointNameFormatter();
                x.AddSagaStateMachine<OrderStateMachine, OrderState>()                
                .InMemoryRepository();
                x.AddSagaStateMachine<OrderCourierStateMachine, OrderTransactionState>()
                .InMemoryRepository();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);                    
                    cfg.Host(massTransitConfig.Host, massTransitConfig.VirtualHost,
                        h =>
                        {
                            h.Username(massTransitConfig.Username);
                            h.Password(massTransitConfig.Password);
                        }
                    );
                });
            });
            services.AddMassTransitHostedService();

            services.AddTransient<IOrderService, OrderService>();
            services.AddScoped<OrderTransactionSubmittedActivity>();
            services.AddOpenApiDocument(cfg => cfg.PostProcess = d => d.Info.Title = "Order Api");
            services.AddControllers();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
