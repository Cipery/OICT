using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OICT.Application;
using OICT.Infrastructure;
using OICT.Infrastructure.Common;
using OICT.Infrastructure.Repositories;

namespace OICT.Api
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
            var connectionString = Configuration.GetConnectionString("OICTApiContext");
            if (Configuration.GetValue<bool>("WaitForDatabaseInit"))
            {
                WaitForDbInit(connectionString);
            }

            services.AddControllers();

            services.AddDbContext<OICTApiContext>(options => options.UseSqlServer(connectionString));

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Employee Microservice API V1", Version = "v1"}); });
            services.AddApplicationServices();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["Jwt:Audience"],
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
                    };
                });
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Microservice API V1");
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            if (Configuration.GetValue<bool>("MigrateDatabaseOnStartup"))
            {
                var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
                using var serviceScope = scopeFactory.CreateScope();
                var serviceProvider = serviceScope.ServiceProvider;
                var dbContext = serviceProvider.GetService<OICTApiContext>();
                StartupDatabase(dbContext);
            }
        }

        public void StartupDatabase(OICTApiContext dbContext)
        {
            Console.WriteLine("Migrating database");
            dbContext.Database.Migrate();
            Console.WriteLine("Migrated database");
            dbContext.SaveChanges();
        }

        private static void WaitForDbInit(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            int retries = 1;
            while (retries < 7)
            {
                try
                {
                    Console.WriteLine("Connecting to db. Trial: {0}, CString: {1}", retries, connectionString);
                    connection.Open();
                    connection.Close();
                    break;
                }
                catch(SqlException)
                {
                    Thread.Sleep((int)Math.Pow(2, retries) * 1000);
                    retries++;
                }
            }
        }
    }
}