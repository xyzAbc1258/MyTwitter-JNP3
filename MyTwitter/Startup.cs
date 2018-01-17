using System;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTwitter.Data;
using MyTwitter.Models;
using MyTwitter.Services;
using Nest;
using ServiceStack.Redis;

namespace MyTwitter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IBusControl>(
                prv => Bus.Factory.CreateUsingRabbitMq(
                    cfg => cfg.Host("queueserver", "/", ps =>
                    {
                        ps.Username("guest"); 
                        ps.Password("guest");
                    })));

            services.AddTransient<ServiceStack.Redis.IRedisClient>(
                p => new RedisManagerPool("redisserver").GetClient());

            services.AddTransient<Services.IRedisClient, Services.RedisClient>();

            new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")).Options).Database.Migrate();
            
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<IQueueClient, QueueClient>();

            var cs = new ConnectionSettings(new Uri("http://elasticsearch:9200"));
            cs.DefaultIndex("default");
            
            services.AddTransient<IElasticClient>(x => new ElasticClient(cs));
            services.AddTransient<IUserFinderService, UserFinderService>();
            services.AddTransient<IUserIndexInsertionService, UserIndexInsertionService>();
            

            services.AddMvc();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
