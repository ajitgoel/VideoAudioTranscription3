using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

// If you rename this namespace, you will need to update the invocation shim
// to match if you intend to test the function with 'amplify mock function'
namespace priceperhour
{
    // If you rename this class, you will need to update the invocation shim
    // to match if you intend to test the function with 'amplify mock function'
    public class priceperhour
    {
      private readonly JsonSerializerSettings jsonSerializerSettings;
      public priceperhour()
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
    public int Get(int noOfHours, ILambdaContext context)
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
      
      #pragma warning disable CS1998
      public async Task<APIGatewayProxyResponse> LambdaHandler(APIGatewayProxyRequest apiGatewayProxyRequest, ILambdaContext context)
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
            context.Logger.LogLine($"apiGatewayProxyRequest: {JsonConvert.SerializeObject(apiGatewayProxyRequest)}\n");
            if (apiGatewayProxyRequest.HttpMethod == "POST")
            {
              var requestBody = apiGatewayProxyRequest.Body;
              var noOfHours = JsonConvert.DeserializeObject<int>(requestBody, jsonSerializerSettings);
              int priceperhour = Get(noOfHours, context);
              apiGatewayProxyResponse.StatusCode = (int)HttpStatusCode.OK;
              apiGatewayProxyResponse.Body = JsonConvert.SerializeObject(new { priceperhour }, jsonSerializerSettings);
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
