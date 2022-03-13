using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace backend
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
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyMethod());
            });
            services.AddControllers();
            services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(Configuration.GetConnectionString("MongoDb")));
            services.AddTransient<IBloodEventRepository, BloodEventRepository>();
            services.AddTransient<IBloodRepository, BloodRepository>();
            services.AddTransient<IHospitalRepository, HospitalRepository>();
            services.AddTransient<IDonorRepository, DonorRepository>();
            services.AddTransient<IDonorTransactionRepository, DonorTransactionRepository>();
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

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
