using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

using Amazon.Lambda.S3Events;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Amazon.TranscribeService;
using System.Linq;
using Amazon.Lambda.DynamoDBEvents;
using AutoFixture;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;

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
      var vocabularies = new Amazon.DynamoDBv2.Model.AttributeValue();
      vocabularies.L = new List<Amazon.DynamoDBv2.Model.AttributeValue>();
      var l1 = new Amazon.DynamoDBv2.Model.AttributeValue
      {
        S = "F.B.I"
      };
      var l2 = new Amazon.DynamoDBv2.Model.AttributeValue
      {
        S = "F.B.I.s"
      };
      var l3 = new Amazon.DynamoDBv2.Model.AttributeValue
      {
        S = "baba"
      };

      vocabularies.L.Add(l1);
      vocabularies.L.Add(l2);
      vocabularies.L.Add(l3);

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

        var transcribeUpdateCustomVocabulary = new TranscribeUpdateCustomVocabulary();
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
