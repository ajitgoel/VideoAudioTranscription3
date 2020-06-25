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
namespace PaymentIntent
{
  // If you rename this class, you will need to update the invocation shim to match if you intend to test the function with 'amplify mock function'
  public class Get
  {
    private readonly JsonSerializerSettings jsonSerializerSettings;
    public Get()
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
    public async Task<APIGatewayProxyResponse> GetPaymentIntent(APIGatewayProxyRequest apiGatewayProxyRequest, ILambdaContext context)
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
          var getPaymentIntentInput = JsonConvert.DeserializeObject<GetPaymentIntentInput>(requestBody, jsonSerializerSettings);

          StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("VUE_APP_STRIPE_SECRET_KEY");
          var paymentIntent = await new PaymentIntentService().GetAsync(getPaymentIntentInput.PaymentIntentId);
          context.Logger.LogLine($"paymentIntent: {JsonConvert.SerializeObject(paymentIntent)}\n");

          var getPaymentIntentOutput = new GetPaymentIntentOutput
          {
            Amount = (paymentIntent?.Charges?.Data?[0]?.Amount/100).ToString(),
            InvoiceDate = paymentIntent?.Created.ToString("MMMM dd yyyy"),            
            PaymentMethod = $"{paymentIntent?.Charges?.Data?[0]?.PaymentMethodDetails?.Card?.Network} - {paymentIntent?.Charges?.Data?[0]?.PaymentMethodDetails?.Card?.Last4}",
            ReceiptUrl = paymentIntent?.Charges?.Data?[0]?.ReceiptUrl,
            ReceiptNumber = paymentIntent?.Charges?.Data?[0]?.ReceiptNumber,
            ReceiptEmail = paymentIntent?.Charges?.Data?[0]?.ReceiptEmail,
            Description = paymentIntent?.Charges?.Data?[0]?.Description,
            Name = paymentIntent?.Charges?.Data?[0]?.BillingDetails?.Name,
            Phone = paymentIntent?.Charges?.Data?[0]?.BillingDetails?.Phone,
            BillingDetailsAddress = paymentIntent?.Charges?.Data?[0]?.BillingDetails?.Address,
          };

          apiGatewayProxyResponse.StatusCode = (int)HttpStatusCode.OK;
          apiGatewayProxyResponse.Body = JsonConvert.SerializeObject(getPaymentIntentOutput, jsonSerializerSettings);
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
  }
}

