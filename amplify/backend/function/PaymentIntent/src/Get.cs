using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Stripe;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Amazon.DynamoDBv2;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
// If you rename this namespace, you will need to update the invocation shim to match if you intend to test the function with 'amplify mock function'
namespace PaymentIntent
{
  public class GetPaymentIntentInput
  {
    [JsonProperty(Required = Required.Always)]
    public string[] PaymentIntentIds { get; set; }
  }
  public class GetPaymentIntentOutput
  {
    public string Id { get; set; }    
    public string InvoiceDate { get; set; }
    public string PaymentMethod { get; set; }
    public string ReceiptUrl { get; set; }
    public string ReceiptNumber { get; set; }
    public string ReceiptEmail { get; set; }
    public object InvoiceData { get; set; }    
    public string Name { get; set; }
    public string Phone { get; set; }
    public Address BillingDetailsAddress { get; set; }
  }

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
    public async Task<APIGatewayProxyResponse> GetPaymentIntents(APIGatewayProxyRequest apiGatewayProxyRequest, ILambdaContext context)
    {
      GetPaymentIntentOutput[] getPaymentIntentOutputs = null;
      try
      {
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
          var getPaymentIntentInput = JsonConvert.DeserializeObject<GetPaymentIntentInput>(requestBody, jsonSerializerSettings);
          var tasks = new List<Task<GetPaymentIntentOutput>>();
          foreach(var paymentIntentId in getPaymentIntentInput.PaymentIntentIds)
          {
            tasks.Add(GetPaymentIntent(paymentIntentId));
          }
          getPaymentIntentOutputs = await Task.WhenAll(tasks);

          apiGatewayProxyResponse.StatusCode = (int)HttpStatusCode.OK;
          apiGatewayProxyResponse.Body = JsonConvert.SerializeObject(getPaymentIntentOutputs, jsonSerializerSettings);
        }
        return apiGatewayProxyResponse;
      }
      catch (Exception exception)
      {
        string logline = $"apiGatewayProxyRequest: {JsonConvert.SerializeObject(apiGatewayProxyRequest)}\n";
        if (getPaymentIntentOutputs != null)
        {
          logline = $"{logline} paymentIntent: {JsonConvert.SerializeObject(getPaymentIntentOutputs)}\n";
        }
        logline = $"{logline} Exception: { exception}";
        context.Logger.LogLine(logline);
        return new APIGatewayProxyResponse
        {
          StatusCode = (int)HttpStatusCode.BadRequest
        };
      }
    }
    /*You can access the following resource attributes as environment variables from your Lambda function
        API_VIDAUDTRANSCRIPTION_GRAPHQLAPIENDPOINTOUTPUT, API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT, ENV, REGION*/
    public async Task<GetPaymentIntentOutput> GetPaymentIntent(string paymentIntentId)
    {
      StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("VUE_APP_STRIPE_SECRET_KEY");
      var amazonDynamoDBClient= new AmazonDynamoDBClient();
      Stripe.PaymentIntent paymentIntent = await new PaymentIntentService().GetAsync(paymentIntentId);
      
      var data = paymentIntent?.Charges?.Data?[0];
      var metadata = data?.Metadata;
      var getPaymentIntentOutput = new GetPaymentIntentOutput
      {
        Id = paymentIntent?.Id,
        InvoiceDate = paymentIntent?.Created.ToString("MMMM dd yyyy"),
        PaymentMethod = $"{data?.PaymentMethodDetails?.Card?.Network} - {data?.PaymentMethodDetails?.Card?.Last4}",
        ReceiptUrl = data?.ReceiptUrl,
        ReceiptNumber = data?.ReceiptNumber,
        ReceiptEmail = data?.ReceiptEmail,
        InvoiceData = new
        {
          Tasks = new[]
          {
            new
            {
              Id=1,
              data?.Description,
              NoOFHours = metadata.ContainsKey("noOFHours") ? metadata["noOFHours"] : "",
              PricePerHour = metadata.ContainsKey("pricePerHour") ? metadata["pricePerHour"] : "",
              DiscountPercentage = metadata.ContainsKey("discountPercentage") ? metadata["discountPercentage"] : "",
              Amount = (data?.Amount/100).ToString(),
            }
          },
          Subtotal = (data?.Amount / 100).ToString(),
          DiscountPercentage = metadata.ContainsKey("discountPercentage") ? metadata["discountPercentage"] : "",
          //DiscountedAmount = 5700,
          Total = (data?.Amount / 100).ToString(),
        },
        Name = data?.BillingDetails?.Name,
        Phone = data?.BillingDetails?.Phone,
        BillingDetailsAddress = data?.BillingDetails?.Address,
      };
      return getPaymentIntentOutput;
    }
  }
}

