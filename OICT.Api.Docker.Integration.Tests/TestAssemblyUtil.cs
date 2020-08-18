using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OICT.Infrastructure;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("OICT.Api.Docker.Integration.Tests.TestAssemblyUtil", "OICT.Api.Docker.Integration.Tests")]
namespace OICT.Api.Docker.Integration.Tests
{
    class TestAssemblyUtil : XunitTestFramework
    {
        public static bool IsContainer { get; private set; }
        public static string BaseApiUrl { get; private set; }
        public TestAssemblyUtil(IMessageSink messageSink)
            : base(messageSink)
        {
            IsContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            BaseApiUrl = Environment.GetEnvironmentVariable("API_URL");
            if (IsContainer)
            {
                WaitForApiToBeAvailable().GetAwaiter().GetResult();
            }
        }

        public new void Dispose()
        {
            base.Dispose();
        }

        private async Task WaitForApiToBeAvailable(int maxTries = 10)
        {
            var baseUrl = BaseApiUrl + "/swagger";
            var client = new HttpClient();
            var retries = 0;
            while (retries < maxTries)
            {
                try
                {
                    Console.WriteLine("Waiting for Api to be available on {0}, retry {1} of {2}", baseUrl, retries, maxTries);
                    await client.GetAsync(baseUrl);
                    Console.WriteLine("Api seems to be responding, starting tests");
                    return;
                }
                catch (HttpRequestException)
                {
                    Thread.Sleep((int)Math.Pow(2, retries) * 1000);
                    retries++;
                }
            }

            throw new Exception("Cannot connect to API");
        }
    }
}
