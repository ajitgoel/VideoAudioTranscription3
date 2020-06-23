using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Models;
using Newtonsoft.Json.Serialization;
using Stripe;
using System.Net.Http;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

// If you rename this namespace, you will need to update the invocation shim
// to match if you intend to test the function with 'amplify mock function'
namespace listallcharges
{
    // If you rename this class, you will need to update the invocation shim
    // to match if you intend to test the function with 'amplify mock function'
    public class listallcharges
    {
      private readonly JsonSerializerSettings jsonSerializerSettings;
      public listallcharges()
      {
        // set jsonSerializerSettings to properly handle Camel-Case Names i.e. the JSON property "fooBar" serializes to the C# property "FooBar"
        DefaultContractResolver contractResolver = new DefaultContractResolver
        {
          NamingStrategy = new CamelCaseNamingStrategy()
        };
        jsonSerializerSettings = new JsonSerializerSettings
        {
          ContractResolver = contractResolver
        };
      }

    /// <summary>
    /// If you rename this function, you will need to update the invocation shim to match if you intend to test the function with 'amplify mock function'
    /// https://stripe.com/docs/api/charges/list
    /// </summary>
    #pragma warning disable CS1998
    public async Task<APIGatewayProxyResponse> LambdaHandler(APIGatewayProxyRequest apiGatewayProxyRequest, ILambdaContext context)
    {
      try
      {
        var apiGatewayProxyResponse = new APIGatewayProxyResponse
        {
          Headers = new Dictionary<string, string> {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept" }
                  }
        };
        context.Logger.LogLine($"apiGatewayProxyRequest: {JsonConvert.SerializeObject(apiGatewayProxyRequest)}\n");
        if (apiGatewayProxyRequest.HttpMethod == "POST")
        {
          var requestBody = apiGatewayProxyRequest.Body;
          var listAllChargesInput = JsonConvert.DeserializeObject<ListAllChargesInput>(requestBody, jsonSerializerSettings);
          var listAllChargesOutput = await GetPaymentReceiptAsync(listAllChargesInput, context);
          apiGatewayProxyResponse.StatusCode = (int)HttpStatusCode.OK;
          apiGatewayProxyResponse.Body = JsonConvert.SerializeObject(listAllChargesOutput, jsonSerializerSettings);
        }
        return apiGatewayProxyResponse;
      }
      catch (Exception exception)
      {
        context.Logger.LogLine($"Exception: {exception}");
        return new APIGatewayProxyResponse
        {
          StatusCode = (int)HttpStatusCode.BadRequest
        };
      }
    }
    private async Task<string> GetPaymentReceiptAsync(ListAllChargesInput listAllChargesInput, ILambdaContext context)
    {
      StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("VUE_APP_STRIPE_SECRET_KEY");//get from environment file.
      var chargeListOptions = new ChargeListOptions { Limit = 1, PaymentIntent = listAllChargesInput.PaymentIntentId };
      var chargeService = new ChargeService();
      var charges = chargeService.List(chargeListOptions);
      var receiptUrl = charges?.First()?.ReceiptUrl;

      var uri = new Uri(receiptUrl);
      var baseUri = uri.GetLeftPart(UriPartial.Authority);
      using var httpClient = new HttpClient
      {
        BaseAddress = new Uri(baseUri)
      };
      httpClient.DefaultRequestHeaders.Accept.Clear();
      httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");
      var httpResponseMessage = await httpClient.GetAsync(uri.PathAndQuery);
      var paymentReceiptBody = string.Empty;
      if (httpResponseMessage.IsSuccessStatusCode)
      {
        var response = await httpResponseMessage.Content.ReadAsStringAsync();
        context.Logger.LogLine($"response: {JsonConvert.SerializeObject(response)}\n");
        var paymentReceipt = JsonConvert.DeserializeObject<PaymentReceipt>(response);
        paymentReceiptBody = paymentReceipt.Body;
      }
      return paymentReceiptBody;
    }
  }
}
