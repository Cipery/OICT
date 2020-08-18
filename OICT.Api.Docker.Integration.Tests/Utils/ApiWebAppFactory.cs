using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OICT.Infrastructure;

namespace OICT.Api.Docker.Integration.Tests.Utils
{
    public class ApiWebAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public string CurrentJwt { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Production");
            builder.ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddInMemoryCollection(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("WaitForDatabaseInit", "false"),
                new KeyValuePair<string, string>("MigrateDatabaseOnStartup", "false"),
            }));
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<OICTApiContext>));

                services.Remove(descriptor);

                services.AddDbContext<OICTApiContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<OICTApiContext>();
                db.Database.EnsureCreated();
            });
        }

        public async Task<string> GetJwt()
        {
            if (CurrentJwt == null)
            {
                CurrentJwt = await QueryApiForJwt();
            }

            return CurrentJwt;
        }

        public async Task<string> QueryApiForJwt()
        {
            var payload = new { Username = "admin", Password = "123456" };
            using var client = CreateHttpClient();
            var result = await client.PostAsync("/api/Auth/login", payload.AsJson());
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<HttpClient> CreateAuthenticatedHttpClientAsync()
        {
            var jwt = await GetJwt();
            var client = CreateHttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);
            return client;
        }

        public HttpClient CreateHttpClient()
        {
            if (TestAssemblyUtil.IsContainer)
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(TestAssemblyUtil.BaseApiUrl)
                };
                return client;
            }

            return CreateClient();
        }
    }
}
