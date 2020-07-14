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
using System;
using Newtonsoft.Json;
using System.Diagnostics;
using Amazon.TranscribeService;
using System.Linq;

namespace transcribeaudiovideo.Tests
{
  public class TranscribeAudioVideoTest
  {
    [Fact]
    public async Task TestS3EventLambdaFunction1()
    {
      var config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
      var aws_access_key_id = config["aws_access_key_id"];
      var aws_secret_access_key=config["aws_secret_access_key"];

      var region = "us-east-1";
      var regionendpoint= RegionEndpoint.GetBySystemName(region);
      var amazonS3Client1 = new AmazonS3Client(aws_access_key_id, aws_secret_access_key, regionendpoint);
      var bucketName = Path.GetRandomFileName().Substring(0, 8);
      var key = "samplevideofile.mp4";
      var fileContentType = "video/mp4";
      var USEnglish="en-US";
      var defaultFileLanguageWhenFileIsTranscribed = USEnglish;
      var useVocabularyWhenFileIsTranscribed = true;
      var notifyWhenTranscriptsCompleted = true;
      var notifyWhenTranscriptsError = true;
      
      try
      {
        using var fileStream = new FileStream(key, FileMode.Open, FileAccess.Read);
        // Create a bucket an object to setup a test data.
        await amazonS3Client1.PutBucketAsync(bucketName);
        var putObjectRequest = new PutObjectRequest
        { BucketName = bucketName, Key = key, ContentType = fileContentType, InputStream = fileStream};
        putObjectRequest.Metadata.Add("defaultFileLanguageWhenFileIsTranscribed", defaultFileLanguageWhenFileIsTranscribed);
        putObjectRequest.Metadata.Add("useVocabularyWhenFileIsTranscribed", useVocabularyWhenFileIsTranscribed.ToString());
        putObjectRequest.Metadata.Add("notifyWhenTranscriptsCompleted", notifyWhenTranscriptsCompleted.ToString());
        putObjectRequest.Metadata.Add("notifyWhenTranscriptsError", notifyWhenTranscriptsError.ToString());

        var putObjectResponse=await amazonS3Client1.PutObjectAsync(putObjectRequest);
        Debug.Print($"putObjectResponse : {Models.Extensions.SerializeObjectIgnoreReferenceLoopHandling(putObjectResponse)}\n");

        // Setup the S3 event object that S3 notifications would create with the fields used by the Lambda function.
        var s3Event = new S3Event
        {
          Records = new List<S3EventNotification.S3EventNotificationRecord>
                  {
                    new S3EventNotification.S3EventNotificationRecord
                    {
                      S3 = new S3EventNotification.S3Entity
                      {
                        Bucket = new S3EventNotification.S3BucketEntity {Name = bucketName },
                        Object = new S3EventNotification.S3ObjectEntity {Key = key }
                      }
                    }
                  }
        };
        var transcribeaudiovideo = new transcribeaudiovideo(amazonS3Client1, region, aws_access_key_id, aws_secret_access_key);
        var contentType = await transcribeaudiovideo.LambdaHandler(s3Event, null);
        Assert.Equal(fileContentType, contentType);
      }
      finally
      {
        //await AmazonS3Util.DeleteS3BucketWithObjectsAsync(amazonS3Client1, bucketName);
      }
    }

    [Fact]
    public void Test2()
    {
      var key = "samplevideofile.mp4";
      var USEnglish = "en-US";
      var languageCode = LanguageCode.FindValue(USEnglish);
      Assert.Equal(languageCode.Value, LanguageCode.EnUS.Value);

      var mediaFormat = MediaFormat.FindValue("mp4");
      Assert.Equal(mediaFormat.Value, MediaFormat.Mp4.Value);
      var extension = key.Split('.').Last();
      Assert.Equal("mp4", extension);

      var file=System.Net.WebUtility.UrlDecode("private/us-east-1%3A5e9adf47-2390-4860-842e-27e75527ce3f/6425f78c-a23d-4a71-aee2-f6b1a0a1e355.mp4");
      Assert.Equal("private/us-east-1:5e9adf47-2390-4860-842e-27e75527ce3f/6425f78c-a23d-4a71-aee2-f6b1a0a1e355.mp4", file);
    }
  }
}
