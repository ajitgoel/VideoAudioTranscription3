using System;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Extensions = Models.Extensions;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using Amazon.TranscribeService.Model;
using Amazon.TranscribeService;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

// If you rename this namespace, you will need to update the invocation shim
// to match if you intend to test the function with 'amplify mock function'
namespace UserProfileTrigger
{
  // If you rename this class, you will need to update the invocation shim
  // to match if you intend to test the function with 'amplify mock function'
  public class UserProfileTrigger
  {
    readonly AmazonS3Client amazonS3Client;
    private readonly AmazonDynamoDBClient amazonDynamoDBClient;
    private readonly AmazonTranscribeServiceClient amazonTranscribeServiceClient;
    private string region;
    private string API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT;
    private string API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN;
    private string storageBucketName;
    private string API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME;

    public UserProfileTrigger()
    {
      region = Environment.GetEnvironmentVariable("REGION");
      API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT = Environment.GetEnvironmentVariable("API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT");
      API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN = Environment.GetEnvironmentVariable("API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN");
      storageBucketName = Environment.GetEnvironmentVariable("STORAGE_S3A41E082F_BUCKETNAME");
      API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME = Environment.GetEnvironmentVariable("API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME");
      amazonS3Client = new AmazonS3Client(RegionEndpoint);
      amazonDynamoDBClient = new AmazonDynamoDBClient(RegionEndpoint);
      amazonTranscribeServiceClient = new AmazonTranscribeServiceClient(RegionEndpoint);
    }
    public UserProfileTrigger(AmazonS3Client amazonS3Client, AmazonDynamoDBClient amazonDynamoDBClient,
      AmazonTranscribeServiceClient amazonTranscribeServiceClient, string region,
      string API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT, string API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN, string storageBucketName,
      string API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME)
    {
      this.region = region;
      this.API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT = API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT;
      this.API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN = API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN;
      this.storageBucketName = storageBucketName;
      this.API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME = API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME;
      this.amazonS3Client = amazonS3Client;
      this.amazonDynamoDBClient = amazonDynamoDBClient;
      this.amazonTranscribeServiceClient = amazonTranscribeServiceClient;
    }
    private RegionEndpoint RegionEndpoint
    {
      get
      {
        return RegionEndpoint.GetBySystemName(region);
      }
    }
    /*API_VIDAUDTRANSCRIPTION_GRAPHQLAPIENDPOINTOUTPUT, API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT
      API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN, API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME, STORAGE_S3A41E082F_BUCKETNAME*/
#pragma warning disable CS1998
    public async Task LambdaHandler(DynamoDBEvent dynamoDBEvent, ILambdaContext iLambdaContext)
    {
      PutObjectRequest putObjectRequest = null;
      IEnumerable<string> vocabularies = null;
      try
      {
        var newImage = dynamoDBEvent?.Records?[0]?.Dynamodb?.NewImage;
        var newImage_vocabularies_L = newImage["vocabularies"]?.L;
        vocabularies = newImage_vocabularies_L.Select(x => x.S);
        var userId = newImage["id"]?.S;
        //store file in https://vidaudtranscriptionb772eac002c6449096461a128cad10533-feature.s3.amazonaws.com/private/us-east-1%3A5c90f58e-f8f4-4189-98ce-a07abb60d230/ folder
        //dynamod id is abebf63e-adcc-486b-a0a2-05700367f194
        // cognito id is 
        //storageBucketName: vidaudtranscriptionb772eac002c6449096461a128cad10533-feature
        string vocabulariesAsString = string.Join(Environment.NewLine, vocabularies.Select(x => x).ToArray());
        putObjectRequest = new PutObjectRequest
        {
          BucketName = storageBucketName,
          Key = $"private/{userId}",
          ContentBody = vocabulariesAsString
        };
        var putObjectResponse = await amazonS3Client.PutObjectAsync(putObjectRequest);
      }
      //catch (Exception exception)
      //{
      //  iLambdaContext?.Logger.LogLine($"exception : {Extensions.SerializeObjectIgnoreReferenceLoopHandling(exception)}\n");
      //  throw;
      //}
      finally
      {
        iLambdaContext?.Logger?.LogLine($"API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT: {API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT} " +
          $"API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN: {API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN} " +
          $"API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME: {API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME} " +
          $"storageBucketName: {storageBucketName} " +
          $"putObjectRequest: {Extensions.SerializeObjectIgnoreReferenceLoopHandling(putObjectRequest)} " +
          $"dynamoDBEvent : {Extensions.SerializeObjectIgnoreReferenceLoopHandling(dynamoDBEvent)}\n" +
          $"vocabularies : {Extensions.SerializeObjectIgnoreReferenceLoopHandling(vocabularies)}\n");
      }
    }

    public async Task CreateUpdateTranscriptionFilter(string userId, ILambdaContext iLambdaContext)
    {
      //iLambdaContext?.Logger.LogLine($"createVocabularyFilterRequest : {Extensions.SerializeObjectIgnoreReferenceLoopHandling(createVocabularyFilterRequest)}\n " +
      //  $"createVocabularyFilterResponse: {Extensions.SerializeObjectIgnoreReferenceLoopHandling(createVocabularyFilterResponse)}");
    }

  }
}
