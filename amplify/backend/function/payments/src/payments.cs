using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Stripe;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

// If you rename this namespace, you will need to update the invocation shim
// to match if you intend to test the function with 'amplify mock function'
namespace payments
{
    // If you rename this class, you will need to update the invocation shim
    // to match if you intend to test the function with 'amplify mock function'
    public class payments
    {
      const string USD_CURRENCY = "usd";
      const string CARD_PAYMENT_METHOD = "card";
      public class Pricing
      {
        public int Id { get; set; }
        public int Priceperhour { get; set; }
        public int Hourmin { get; set; }
        public int Hourmax { get; set; }
      }
      public class CreatePaymentIntentInput
      {
        [JsonProperty(Required = Required.Always)]
        public int NoOFHours { get; set; }
        [JsonProperty(Required = Required.Always)]
        public bool AutoRecharge { get; set; }=false;
        [JsonProperty(Required = Required.Always)]
        public string Email { get; set; }
      }
      public class CreatePaymentIntentOutput
      {
        [JsonProperty(Required = Required.Always)]
        public string ClientSecret { get; set; }
      }
      private readonly JsonSerializerSettings jsonSerializerSettings;
      public payments()
      {
        // set jsonSerializerSettings to properly handle Camel-Case Names
        //  i.e. the JSON property "fooBar" serializes to the C# property "FooBar"
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
      /// </remarks>
      #pragma warning disable CS1998
      public async Task<APIGatewayProxyResponse> CreatePaymentIntent(APIGatewayProxyRequest apiGatewayProxyRequest, ILambdaContext context)
      {
        try
        {
          //https://stripe.com/docs/payments/accept-a-payment
          var apiGatewayProxyResponse = new APIGatewayProxyResponse
          {
            Headers = new Dictionary<string, string> {
                      { "Access-Control-Allow-Origin", "*" },
                      { "Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept" }
                    }
          };
          if (apiGatewayProxyRequest.HttpMethod == "POST")
          {
            var requestBody=apiGatewayProxyRequest.Body;
            var singlePaymentInput = JsonConvert.DeserializeObject<CreatePaymentIntentInput>(requestBody, jsonSerializerSettings);
            
            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("VUE_APP_STRIPE_SECRET_KEY");//get from environment file.
            var pricePerHour=GetPricePerHour(singlePaymentInput.NoOFHours, context);
            context.Logger.LogLine($"Amount Charged: { pricePerHour * singlePaymentInput.NoOFHours * 100}");
            var paymentIntentCreateOptions = new PaymentIntentCreateOptions
            {
              Amount = pricePerHour * singlePaymentInput.NoOFHours * 100,
              Currency = USD_CURRENCY,
              PaymentMethodTypes = new List<string> {CARD_PAYMENT_METHOD },
              Description="Top off payment for video audio transcription",
              ReceiptEmail = singlePaymentInput.Email,
              Metadata = new Dictionary<string, string>
              {
                { "integration_check", "accept_a_payment" },
              },
            };
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(paymentIntentCreateOptions);
            apiGatewayProxyResponse.StatusCode = (int)HttpStatusCode.OK;
            apiGatewayProxyResponse.Body = JsonConvert.SerializeObject(new CreatePaymentIntentOutput { ClientSecret=paymentIntent.ClientSecret }, jsonSerializerSettings);
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

      private int GetPricePerHour(int noOfHours, ILambdaContext context)
      {
        int priceperhour = 0;

        try
        {
          var pricings = new List<Pricing>
              {
                new Pricing{Id= 1, Priceperhour= 10, Hourmin=0, Hourmax= 24,},
                new Pricing{Id= 2, Priceperhour= 9, Hourmin=25, Hourmax= 49,},
                new Pricing{Id= 3, Priceperhour= 8, Hourmin=50, Hourmax= 100,}
              };
          for (var index = 0; index < pricings.Count; index++)
          {
            var element = pricings[index];
            if (element.Hourmin <= noOfHours && element.Hourmax >= noOfHours)
            {
              priceperhour = element.Priceperhour;
            }
          }
          return priceperhour;
        }
        catch (Exception exception)
        {
          context.Logger.LogLine($"Exception: {exception}");
          return priceperhour;
        }
      }

      public async Task<APIGatewayProxyResponse> GetPricePerHour(APIGatewayProxyRequest apiGatewayProxyRequest, ILambdaContext context)
        { 
          try
          {
            //https://stripe.com/docs/payments/accept-a-payment
            var apiGatewayProxyResponse = new APIGatewayProxyResponse
            {
              Headers = new Dictionary<string, string> {
                          { "Access-Control-Allow-Origin", "*" },
                          { "Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept" }
                        }
            };
            if (apiGatewayProxyRequest.HttpMethod == "POST")
            {
              var requestBody = apiGatewayProxyRequest.Body;
              var noOfHours = JsonConvert.DeserializeObject<int>(requestBody, jsonSerializerSettings);
              int priceperhour = GetPricePerHour(noOfHours, context);
              apiGatewayProxyResponse.StatusCode = (int)HttpStatusCode.OK;
              apiGatewayProxyResponse.Body = JsonConvert.SerializeObject(new { priceperhour}, jsonSerializerSettings);
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
