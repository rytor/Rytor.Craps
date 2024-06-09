using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rytor.Craps.Microservices.Balance.Interfaces;
using Rytor.Craps.Microservices.Balance.Repositories;

namespace Rytor.Craps.Microservices.Balance
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
            string dbHost = System.Environment.GetEnvironmentVariable("DB_HOST");
            string dbPort = System.Environment.GetEnvironmentVariable("DB_PORT");
            string dbUser = System.Environment.GetEnvironmentVariable("DB_USER");
            string dbPassword = System.Environment.GetEnvironmentVariable("DB_PASSWORD");
            string dbName = "balance";
            string connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};User ID={dbUser};Password={dbPassword}";

            services.AddSingleton<IBalanceRepository>(x => new BalanceRepository(connectionString, x.GetService<ILoggerFactory>()));
            services.AddSingleton<IActivityRepository>(x => new ActivityRepository(connectionString, x.GetService<ILoggerFactory>()));

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
