using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

// If you rename this namespace, you will need to update the invocation shim
// to match if you intend to test the function with 'amplify mock function'
namespace transcribeaudiovideo
{
    // If you rename this class, you will need to update the invocation shim
    // to match if you intend to test the function with 'amplify mock function'
    public class transcribeaudiovideo
    {
        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        /// <remarks>
        /// If you rename this function, you will need to update the invocation shim
        /// to match if you intend to test the function with 'amplify mock function'
        /// </remarks>
        #pragma warning disable CS1998
        public async Task LambdaHandler(S3Event s3Event, ILambdaContext context)
        {
            context.Logger.LogLine($"s3Event: {JsonConvert.SerializeObject(s3Event)}\n");
            return;
        }
    }
}
