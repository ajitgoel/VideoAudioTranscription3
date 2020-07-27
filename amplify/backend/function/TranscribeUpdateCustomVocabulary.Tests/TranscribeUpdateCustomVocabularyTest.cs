using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.Lambda.DynamoDBEvents;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using Amazon.S3;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.TranscribeService;

namespace TranscribeUpdateCustomVocabulary.Tests
{
  public class TranscribeUpdateCustomVocabularyTest
  {
    [Fact]
    public async Task Test_TranscribeUpdateCustomVocabulary_LambdaFunction1()
    {
      var credentialProfileStoreChain = new CredentialProfileStoreChain(@"C:\Users\AjitGoel\.aws\credentials");
      credentialProfileStoreChain.TryGetAWSCredentials("amplify-workshop-user", out AWSCredentials awsCredentials);
      var credentials = awsCredentials.GetCredentials();
      var aws_access_key_id = credentials.AccessKey;
      var aws_secret_access_key = credentials.SecretKey;
      var newimage=new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>();
      var vocabularies = new Amazon.DynamoDBv2.Model.AttributeValue
      {
        L = new List<Amazon.DynamoDBv2.Model.AttributeValue>()
      };
      vocabularies.L.Add(new Amazon.DynamoDBv2.Model.AttributeValue{S = "F.B.I"});
      vocabularies.L.Add(new Amazon.DynamoDBv2.Model.AttributeValue { S = "F.B.I.s" });
      vocabularies.L.Add(new Amazon.DynamoDBv2.Model.AttributeValue { S = "baba" });

      newimage.Add("vocabularies", vocabularies);

      try
      {        
        var dynamoDBEvent = new DynamoDBEvent
        {
          Records = new List<DynamoDBEvent.DynamodbStreamRecord>()
            {
              new DynamoDBEvent.DynamodbStreamRecord()
              {
                Dynamodb=new  Amazon.DynamoDBv2.Model.StreamRecord()
                {
                  NewImage= newimage
                }
              }
            }          
        };

        var region = "us-east-1";
        var regionendpoint = RegionEndpoint.GetBySystemName(region);
        AmazonS3Client amazonS3Client =new AmazonS3Client(aws_access_key_id,aws_secret_access_key, regionendpoint);
        AmazonDynamoDBClient amazonDynamoDBClient = null;
        AmazonTranscribeServiceClient amazonTranscribeServiceClient = null;
        string API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT=string.Empty;
        string API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN = string.Empty;
        string storageBucketName = string.Empty;
        string API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME = string.Empty;
        var transcribeUpdateCustomVocabulary = new UserProfileTrigger.UserProfileTrigger(
          amazonS3Client, amazonDynamoDBClient, amazonTranscribeServiceClient, region, API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT,
          API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN, storageBucketName, API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME);
        await transcribeUpdateCustomVocabulary.LambdaHandler(dynamoDBEvent, null);
        //Assert.Equal(fileContentType, contentType);
      }
      finally
      {
        //await AmazonS3Util.DeleteS3BucketWithObjectsAsync(amazonS3Client1, bucketName);
      }
    }

    [Fact]
    public async Task AddUpdateUnwantedWordsTest()
    {
      var credentialProfileStoreChain = new CredentialProfileStoreChain(@"C:\Users\AjitGoel\.aws\credentials");
      credentialProfileStoreChain.TryGetAWSCredentials("amplify-workshop-user", out AWSCredentials awsCredentials);
      var credentials = awsCredentials.GetCredentials();
      var aws_access_key_id = credentials.AccessKey;
      var aws_secret_access_key = credentials.SecretKey;
      var newimage = new Dictionary<string, Amazon.DynamoDBv2.Model.AttributeValue>();
      var vocabularies = new Amazon.DynamoDBv2.Model.AttributeValue
      {
        L = new List<Amazon.DynamoDBv2.Model.AttributeValue>()
      };
      vocabularies.L.Add(new Amazon.DynamoDBv2.Model.AttributeValue { S = "F.B.I" });
      vocabularies.L.Add(new Amazon.DynamoDBv2.Model.AttributeValue { S = "F.B.I.s" });
      vocabularies.L.Add(new Amazon.DynamoDBv2.Model.AttributeValue { S = "baba" });

      newimage.Add("vocabularies", vocabularies);

      try
      {
        var dynamoDBEvent = new DynamoDBEvent
        {
          Records = new List<DynamoDBEvent.DynamodbStreamRecord>()
            {
              new DynamoDBEvent.DynamodbStreamRecord()
              {
                Dynamodb=new  Amazon.DynamoDBv2.Model.StreamRecord()
                {
                  NewImage= newimage
                }
              }
            }
        };

        var region = "us-east-1";
        var regionendpoint = RegionEndpoint.GetBySystemName(region);
        AmazonS3Client amazonS3Client = new AmazonS3Client(aws_access_key_id, aws_secret_access_key, regionendpoint);
        AmazonDynamoDBClient amazonDynamoDBClient = null;
        AmazonTranscribeServiceClient amazonTranscribeServiceClient = null;
        string API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT = string.Empty;
        string API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN = string.Empty;
        string storageBucketName = string.Empty;
        string API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME = string.Empty;
        var transcribeUpdateCustomVocabulary = new UserProfileTrigger.UserProfileTrigger(
          amazonS3Client, amazonDynamoDBClient, amazonTranscribeServiceClient, region, API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT,
          API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_ARN, storageBucketName, API_VIDAUDTRANSCRIPTION_USERPROFILETABLE_NAME);
        await transcribeUpdateCustomVocabulary.LambdaHandler(dynamoDBEvent, null);
        //Assert.Equal(fileContentType, contentType);
      }
      finally
      {
        //await AmazonS3Util.DeleteS3BucketWithObjectsAsync(amazonS3Client1, bucketName);
      }
    }
  }
}
