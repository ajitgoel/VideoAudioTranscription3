using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using System.Net;
using System.IO;
using System.Text;
using Amazon.S3.Transfer;

using Newtonsoft.Json.Linq;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using Newtonsoft.Json;
using Amazon;
using Models;

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
    /*You can access the following resource attributes as environment variables from your Lambda function
        API_VIDAUDTRANSCRIPTION_GRAPHQLAPIENDPOINTOUTPUT, API_VIDAUDTRANSCRIPTION_GRAPHQLAPIIDOUTPUT, ENV, REGION
    Environment.GetEnvironmentVariable("VUE_APP_STRIPE_SECRET_KEY");*/
    private RegionEndpoint RegionEndpoint
    {
      get
      {
        return RegionEndpoint.GetBySystemName(region);
      }
    }
    private string region;
    private IAmazonS3 amazonS3Client1;
    private readonly AmazonTranscribeServiceClient amazonTranscribeServiceClient;
    public transcribeaudiovideo()
    {
      region = Environment.GetEnvironmentVariable("REGION");
      amazonTranscribeServiceClient = new AmazonTranscribeServiceClient(RegionEndpoint);
      amazonS3Client1 = new AmazonS3Client(RegionEndpoint);
    }
    public transcribeaudiovideo(IAmazonS3 iAmazonS3, string region, string aws_access_key_id, string aws_secret_access_key)
    {
      this.region = region;
      this.amazonTranscribeServiceClient = new AmazonTranscribeServiceClient(aws_access_key_id, aws_secret_access_key, RegionEndpoint);
      this.amazonS3Client1 = iAmazonS3;
    }
    public async Task<string> LambdaHandler(S3Event environment, ILambdaContext iLambdaContext)
    {
      var s3Event = environment.Records?[0].S3;
        
      if (s3Event == null)
      {
          return null;
      }
      try
      {
        var filename = WebUtility.UrlDecode(s3Event.Object.Key);

        var fileExtension= filename.Split('.').Last();
        iLambdaContext?.Logger.LogLine($"transcribeaudiovideo: environment : " +
          $"{Models.Extensions.SerializeObjectIgnoreReferenceLoopHandling(environment)}\n");
        var getObjectMetadataResponse = await this.amazonS3Client1.GetObjectMetadataAsync(s3Event.Bucket.Name, filename);
        iLambdaContext?.Logger.LogLine($"getObjectMetadataResponse : " +
          $"{Models.Extensions.SerializeObjectIgnoreReferenceLoopHandling(getObjectMetadataResponse)}\n");

        var keyprefix = "x-amz-meta-";
        var defaultFileLanguageWhenFileIsTranscribed = getObjectMetadataResponse.Metadata[$"{keyprefix}defaultfilelanguagewhenfileistranscribed"];
        var useVocabularyWhenFileIsTranscribed = getObjectMetadataResponse.Metadata[$"{keyprefix}usevocabularywhenfileistranscribed"];
        var notifyWhenTranscriptsCompleted = getObjectMetadataResponse.Metadata[$"{keyprefix}notifywhentranscriptscompleted"];
        var notifyWhenTranscriptsError = getObjectMetadataResponse.Metadata[$"{keyprefix}notifywhentranscriptserror"];

        if(defaultFileLanguageWhenFileIsTranscribed == null || useVocabularyWhenFileIsTranscribed == null ||
          notifyWhenTranscriptsCompleted == null||notifyWhenTranscriptsError ==null)
        {
          throw new Exception("Error with passed parameters");
        }  

        var languageCode = LanguageCode.FindValue(defaultFileLanguageWhenFileIsTranscribed);
        var mediaFormat = MediaFormat.FindValue(fileExtension);

        await ProcessTranscribe(s3Event.Bucket.Name, s3Event.Object.Key, languageCode, mediaFormat, iLambdaContext);
        return getObjectMetadataResponse.Headers.ContentType;
      }
      catch (Exception exception)
      {
          iLambdaContext?.Logger.LogLine($"transcribeaudiovideo: exception : " +
            $"{Models.Extensions.SerializeObjectIgnoreReferenceLoopHandling(exception)}\n");
          throw;
      }
    }
    private async Task ProcessTranscribe(string bucket, string key, LanguageCode languageCode, MediaFormat mediaFormat,
      ILambdaContext iLambdaContext)
    {
      var settings = new Settings
      {
        ShowSpeakerLabels = true,
        MaxSpeakerLabels = 2,
        //VocabularyName = "Vocab"
      };
      var media = new Media
      {
        MediaFileUri = $"https://s3.{region}.amazonaws.com/{bucket}/{key}" 
      };
      var cancellationToken = new CancellationToken();
      var startTranscriptionJobRequest = new StartTranscriptionJobRequest
      {
        LanguageCode = languageCode,
        Settings = settings,
        Media = media,
        MediaFormat = mediaFormat,
        TranscriptionJobName = Guid.NewGuid().ToString()
      };

      iLambdaContext?.Logger.LogLine($"startTranscriptionJobRequest : " +
        $"{Models.Extensions.SerializeObjectIgnoreReferenceLoopHandling(startTranscriptionJobRequest)}\n");

      var startTranscriptionJobResponse =
        await amazonTranscribeServiceClient.StartTranscriptionJobAsync(startTranscriptionJobRequest, cancellationToken);
      iLambdaContext?.Logger.LogLine($"startTranscriptionJobResponse : " +
        $"{Models.Extensions.SerializeObjectIgnoreReferenceLoopHandling(startTranscriptionJobResponse)}\n");

      var getTranscriptionJobRequest = new GetTranscriptionJobRequest
      {
        TranscriptionJobName = startTranscriptionJobRequest.TranscriptionJobName
      };
      bool isComplete = false;
      while (isComplete== false)
      {
        var getTranscriptionJobResponse2 =
          await amazonTranscribeServiceClient.GetTranscriptionJobAsync(getTranscriptionJobRequest);
        iLambdaContext?.Logger.LogLine($"getTranscriptionJobResponse2 : " +
          $"{Models.Extensions.SerializeObjectIgnoreReferenceLoopHandling(getTranscriptionJobResponse2)}\n");

        if (getTranscriptionJobResponse2.TranscriptionJob.TranscriptionJobStatus == TranscriptionJobStatus.COMPLETED)
        {
            isComplete = true;
            WriteTranscriptionOutputToS3(getTranscriptionJobResponse2.TranscriptionJob.Transcript.TranscriptFileUri,
              startTranscriptionJobRequest.TranscriptionJobName, bucket);
        }
        else if (getTranscriptionJobResponse2.TranscriptionJob.TranscriptionJobStatus == TranscriptionJobStatus.FAILED)
        {
          isComplete = true;
          iLambdaContext?.Logger.LogLine(
            $"ProcessTranscribe: exception : {Models.Extensions.SerializeObjectIgnoreReferenceLoopHandling(getTranscriptionJobResponse2)}\n");
        }
        else
        {
            Thread.Sleep(1000);
        }
      }
    }
    private void WriteTranscriptionOutputToS3(string transcriptURI, string jobName, string bucket)
    {
      var webRequest = WebRequest.Create(transcriptURI);
      string strContent;
      using (var webResponse = webRequest.GetResponse())
      using (var responseStream = webResponse.GetResponseStream())
      using (var streamReader = new StreamReader(responseStream))
      {
          strContent = streamReader.ReadToEnd();
      }
      TransformFileToS3(strContent, jobName, bucket);
      WriteRawFileToS3(strContent, jobName, bucket);
    }
    private bool WriteRawFileToS3(string strContent, string jobName, string bucket)
    {
      var bytes = Encoding.UTF8.GetBytes(strContent);
      using var memoryStream = new MemoryStream(strContent.Length);
      memoryStream.Write(bytes, 0, bytes.Count());
      using var transferUtility = new TransferUtility(amazonS3Client1);
      //make changes
      transferUtility.Upload(memoryStream, bucket, $"raw-transcripts{jobName}.json");
      return true;      
    }
    private bool TransformFileToS3(string strContent, string jobName, string bucket)
    {
      var transcriptJSON = JObject.Parse(strContent);
      var sendObject = new JObject(
          new JProperty("JobName", (string)transcriptJSON["jobName"]),
          new JProperty("Text", (string)transcriptJSON["results"]["transcripts"][0]["transcript"]));
      // Create file in memory
      var memstring = Encoding.UTF8.GetBytes(sendObject.ToString());
      using Stream memStream = new MemoryStream(strContent.Length);
      memStream.Write(memstring, 0, memstring.Count());
      using var transferUtility = new TransferUtility(amazonS3Client1);
      transferUtility.Upload(memStream, bucket, $"transcripts-{jobName}.json");
      return true;      
    }
  }
}
