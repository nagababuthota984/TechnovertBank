using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnovertBank.API.Handlers;
using TechnovertBank.Data;
using TechnovertBank.Services;
using Newtonsoft.Json;

namespace TechnovertBank.API
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
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<BankStorageContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BankStorageDatabase")), ServiceLifetime.Scoped);
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddAuthentication("BasiAuthentication")
                //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasiAuthentication",null);
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

            app.UseAuthentication();      

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
