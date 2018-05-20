using System;
using TechTalk.SpecFlow;
using RestSharp;
using System.Collections.Generic;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Specflow.RestSharp.Features.Support;

namespace Specflow.RestSharp.Features.StepDefenitions

{
    [Binding]
    public class ApiSteps
    {
        private readonly RestClient client;
        private readonly String baseUrl;

        private string endPoint;
        private Dictionary<string, string> headers;

        private RestRequest request;
        private IRestResponse response;

        public ApiSteps(Hooks hooks)
        {
            this.client = hooks.GetRestClient();
            this.baseUrl = hooks.getBaseUrl();
        }

        [Given(@"path '(.*)'")]
        public void Path(string endpoint)
        {
            this.endPoint = endpoint;
        }

        [When(@"method GET")]
        public void MethodGet()
        {
            this.request = new RestRequest(endPoint, Method.GET);
        }

        [When(@"method POST")]
        public void MethodPost()
        {
            this.request = new RestRequest(endPoint, Method.POST);
        }

        [When(@"method PUT")]
        public void MethodPut()
        {
            this.request = new RestRequest(endPoint, Method.PUT);
        }

        [When(@"method DELETE")]
        public void MethodDelete()
        {
            this.request = new RestRequest(endPoint, Method.DELETE);
        }

        [When(@"headers")]
        public void Headers(Table table)
        {
            this.headers = Utils.Utilities.ToDictionary(table);
            foreach (KeyValuePair<string, String> header in headers)
            {
                this.request.AddHeader(header.Key, header.Value);
            }
        }

        [When(@"payload ""(.*)""")]
        public void Payload(String className, String payloadModification)
        {
            Object payload = Utils.Utilities.CreatePayload(className, payloadModification);
            this.request.AddJsonBody(payload);
        }

        [Then(@"status ""(.*)""")]
        public void Status(String statusCode)
        {
            this.response = client.Execute(request);
            int responseStatusCode = (int)response.StatusCode;

            Assert.AreEqual(responseStatusCode.ToString(), statusCode, String.Format("The response status code {0}, did match match expected code {1}", responseStatusCode, statusCode));
        }

        [Then(@"the schema is correct ""(.*)""")]
        public void ThenTheSchemaIsCorrect(string schemaName)
        {
            JObject responseBody = JObject.Parse(response.Content);
            Boolean isValid = Utils.Utilities.CompareSchema(schemaName, responseBody);
            Assert.IsTrue(isValid, String.Format("Response does not match {0}", schemaName));
        }
    }
}
