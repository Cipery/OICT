using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("OICT.Api.Docker.Integration.Tests.TestAssemblyInitializer", "OICT.Api.Docker.Integration.Tests")]
namespace OICT.Api.Docker.Integration.Tests
{
    class TestAssemblyInitializer : XunitTestFramework
    {
        public TestAssemblyInitializer(IMessageSink messageSink)
            : base(messageSink)
        {
            WaitForApiToBeAvailable().GetAwaiter().GetResult();
        }

        public new void Dispose()
        {
            // Place tear down code here
            base.Dispose();
        }

        private async Task WaitForApiToBeAvailable(int maxTries = 10)
        {
            var baseUrl = Environment.GetEnvironmentVariable("API_URL") + "/swagger";
            var client = new HttpClient();
            var retries = 0;
            while (retries < maxTries)
            {
                try
                {
                    Console.WriteLine("Waiting for Api to be available, retry {0} of {1}", retries, maxTries);
                    await client.GetAsync(baseUrl);
                    Console.WriteLine("Api seems to be responding, starting tests");
                    break;
                }
                catch (HttpRequestException e)
                {
                    Thread.Sleep((int)Math.Pow(2, retries) * 1000);
                    retries++;
                }
            }
        }
    }
}
