using RestSharp;
using System;
using TechTalk.SpecFlow;

namespace Specflow.RestSharp.Features.Support
{
    [Binding]
    public sealed class Hooks
    {
        private RestClient client;
        private static String baseUrl;

        public RestClient GetRestClient()
        {
            return client;
        }

        public String getBaseUrl()
        {
            return baseUrl;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            baseUrl = Environment.GetEnvironmentVariable("baseUrl");
            if(baseUrl == null)
            {
                Environment.SetEnvironmentVariable("baseUrl", "https://api.postcodes.io/");
                baseUrl = Environment.GetEnvironmentVariable("baseUrl");
            }
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            this.client = new RestClient();
            client.BaseUrl = new Uri(baseUrl);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            client = null;
        }
    }
}
