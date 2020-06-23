using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Stripe;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
// If you rename this namespace, you will need to update the invocation shim to match if you intend to test the function with 'amplify mock function'
namespace payments
{
    // If you rename this class, you will need to update the invocation shim to match if you intend to test the function with 'amplify mock function'
    public class payments
    {      
      private readonly JsonSerializerSettings jsonSerializerSettings;
      public payments()
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
    /// <remarks>
    /// If you rename this function, you will need to update the invocation shim to match if you intend to test the function with 'amplify mock function'
    /// https://stripe.com/docs/payments/accept-a-payment
    /// </remarks>
    #pragma warning disable CS1998
    public async Task<APIGatewayProxyResponse> CreatePaymentIntent(APIGatewayProxyRequest apiGatewayProxyRequest, ILambdaContext context)
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
          var requestBody=apiGatewayProxyRequest.Body;
          var createPaymentIntentInput = JsonConvert.DeserializeObject<CreatePaymentIntentInput>(requestBody, jsonSerializerSettings);
          var createPaymentIntentOutput =CreatePaymentIntent(createPaymentIntentInput);
          apiGatewayProxyResponse.StatusCode = (int)HttpStatusCode.OK;
          apiGatewayProxyResponse.Body = JsonConvert.SerializeObject(createPaymentIntentOutput, jsonSerializerSettings);
        }
        return apiGatewayProxyResponse;
      }
      catch(Exception exception)
      {
        context.Logger.LogLine($"Exception: {exception}");
        return new APIGatewayProxyResponse
        {
          StatusCode = (int)HttpStatusCode.BadRequest
        };
      }          
    }

    private CreatePaymentIntentOutput CreatePaymentIntent(CreatePaymentIntentInput createPaymentIntentInput)
    {
      StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("VUE_APP_STRIPE_SECRET_KEY");
      var pricePerHour = new Methods().GetPricePerHour(createPaymentIntentInput.NoOFHours);
      var paymentIntentCreateOptions = new PaymentIntentCreateOptions
      {
        Amount = pricePerHour * createPaymentIntentInput.NoOFHours * 100,
        Currency = Constants.USD_CURRENCY,
        PaymentMethodTypes = new List<string> { Constants.CARD_PAYMENT_METHOD },
        Description = "Top off payment for video audio transcription",
        ReceiptEmail = createPaymentIntentInput.Email,
        Metadata = new Dictionary<string, string>
        {
          { "integration_check", "accept_a_payment" },
        },
      };
      var paymentIntentService = new PaymentIntentService();
      var paymentIntent = paymentIntentService.Create(paymentIntentCreateOptions);
      return new CreatePaymentIntentOutput { ClientSecret = paymentIntent.ClientSecret };
    }
  }
}
