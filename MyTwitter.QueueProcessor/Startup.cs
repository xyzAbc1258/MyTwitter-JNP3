using System;
using System.IO;
using System.Linq;
using System.Net;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.DependencyInjection;


namespace MyTwitter.QueueProcessor
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {   
            services.AddSingleton<IBusControl>(sc => Bus.Factory.CreateUsingRabbitMq(c =>
                    {
                        var host = c.Host("queueserver", "/",
                            cf =>
                            {
                                cf.Username("guest");
                                cf.Password("guest");
                            });
                        c.ReceiveEndpoint(host, "messagesA",x => x.Consumer<AddConsumer>());
                        c.ReceiveEndpoint(host, "messagesU",x => x.Consumer<UpdateConsumer>());
                    }
                )
            );
            services.AddSingleton<IBus>(sc => sc.GetService<IBusControl>());
            services.AddSingleton<IPublishEndpoint>(sc => sc.GetService<IBus>());

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var bus = app.ApplicationServices.GetService<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            IApplicationLifetime lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();
            lifetime.ApplicationStopping.Register(bus.Stop);
        }
    }
}
