using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using backend.Controllers;
using backend.Repositories;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            var env = DotEnv.Read();
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyMethod());
            });
            services.AddControllers();
            // services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(Configuration.GetConnectionString("MongoDb")));
            services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(env["MongoDB"]));
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<IBloodRepository, BloodRepository>();
            services.AddTransient<IHospitalRepository, HospitalRepository>();
            services.AddTransient<IDonorRepository, DonorRepository>();
            services.AddTransient<IRequestRepository, RequestRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IDonorTransactionRepository, DonorTransactionRepository>();
            services.AddTransient<IEventSubmissionRepository, EventSubmissionRepository>();
            services.AddTransient<IRecentActivityRepository, RecentActivityRepository>();

            //JWT Authentication
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSpaStaticFiles();
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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            if (Directory.Exists("wwwroot/admin") && Directory.Exists("wwwroot/landing-page"))
            {
                var provider = new FileExtensionContentTypeProvider();

                app.MapWhen(
                    x => x.Request.Path.Value != null && x.Request.Path.StartsWithSegments(new PathString("/admin")),
                    builder =>
                    {
                        builder.UseStaticFiles(new StaticFileOptions()
                        {
                            FileProvider =
                                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                            ContentTypeProvider = provider
                        });
                        builder.UseSpaStaticFiles(new StaticFileOptions()
                        {
                            FileProvider =
                                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                            ContentTypeProvider = provider
                        });
                        builder.UseSpa(spa =>
                        {
                            spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                            {
                                FileProvider =
                                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                                ContentTypeProvider = provider
                            };
                            spa.Options.SourcePath = "wwwroot";
                            spa.Options.DefaultPage = "/admin/index.html";
                        });
                    });

                app.MapWhen(
                    x => x.Request.Path.Value != null && !x.Request.Path.StartsWithSegments(new PathString("/admin")),
                    builder =>
                    {
                        builder.UseStaticFiles(new StaticFileOptions()
                        {
                            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot", "landing-page")),
                            ContentTypeProvider = provider
                        });
                        builder.UseSpaStaticFiles(new StaticFileOptions()
                        {
                            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot", "landing-page")),
                            ContentTypeProvider = provider
                        });
                        builder.UseSpa(spa =>
                        {
                            spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                            {
                                FileProvider =
                                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                                        "landing-page")),
                                ContentTypeProvider = provider
                            };
                            spa.Options.SourcePath = "wwwroot/landing-page";
                            spa.Options.DefaultPage = "/index.html";
                        });
                    });
            }
        }
    }
}