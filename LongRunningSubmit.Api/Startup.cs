using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LongRunningSubmit.Api.BackgroundServices;
using LongRunningSubmit.Api.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LongRunningSubmit.Api
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
            services.AddCors(options => {
                options.AddPolicy(name: "AllowAll", builder => {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed(x => true)
                    .WithOrigins("http://loscalhost:8100");
                });
            });

            services.AddSignalR();

            services.AddControllers();

            services.AddHostedService<LongRunningSubmitBackgroundService>();
            services.AddSingleton<LongRunningSubmitBackgroundService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // app.UseSignalR(builder =>{ 
                
            //     builder.MapHub<NotificationHub>("/notification"); 
            // });
                
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHub<NotificationHub>("/hub/notifications");
            });
        }
    }
}
