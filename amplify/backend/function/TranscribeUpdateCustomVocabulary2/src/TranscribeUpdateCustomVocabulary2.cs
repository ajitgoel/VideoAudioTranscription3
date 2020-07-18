using System;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Extensions = Models.Extensions;
using System.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

// If you rename this namespace, you will need to update the invocation shim
// to match if you intend to test the function with 'amplify mock function'
namespace TranscribeUpdateCustomVocabulary2
{
  // If you rename this class, you will need to update the invocation shim
  // to match if you intend to test the function with 'amplify mock function'
  public class TranscribeUpdateCustomVocabulary2
  {
    /*API_VIDAUDTRANSCRIPTION_GRAPHQLAPIENDPOINTOUTPUT, API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT
      API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN, API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME, STORAGE_S3A41E082F_BUCKETNAME*/
#pragma warning disable CS1998
    public async Task LambdaHandler(DynamoDBEvent dynamoDBEvent, ILambdaContext iLambdaContext)
    {
      var newImage = dynamoDBEvent?.Records?[0]?.Dynamodb?.NewImage;
      var attributeValues = newImage["vocabularies"]?.L;
      var vocabularies = attributeValues.Select(x => x.S);
      var storageBucketName = Environment.GetEnvironmentVariable("STORAGE_S3A41E082F_BUCKETNAME");

      iLambdaContext?.Logger?.LogLine($"storageBucketName: {storageBucketName} dynamoDBEvent : " +
        $"{Extensions.SerializeObjectIgnoreReferenceLoopHandling(dynamoDBEvent)}\n");
      iLambdaContext?.Logger?.LogLine($"vocabularies : " +
        $"{Extensions.SerializeObjectIgnoreReferenceLoopHandling(vocabularies)}\n");
    }
  }
}
