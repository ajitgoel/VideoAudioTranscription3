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
              var pricing = new Methods().GetPricePerHour(noOfHours);
              apiGatewayProxyResponse.StatusCode = (int)HttpStatusCode.OK;
              apiGatewayProxyResponse.Body = JsonConvert.SerializeObject(pricing, jsonSerializerSettings);
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
