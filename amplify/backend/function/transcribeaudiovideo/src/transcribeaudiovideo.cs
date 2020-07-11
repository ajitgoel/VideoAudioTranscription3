using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Newtonsoft.Json;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using Amazon;
using System;

// If you rename this namespace, you will need to update the invocation shim
// to match if you intend to test the function with 'amplify mock function'
namespace transcribeaudiovideo
{
    // If you rename this class, you will need to update the invocation shim
    // to match if you intend to test the function with 'amplify mock function'
    public class transcribeaudiovideo
    {        
        private string _sourceBucket = "";
        private string _writeBucket = "";
        private string _prefix = "";
        private IAmazonTranscribeService iAmazonTranscribeService;

        #pragma warning disable CS1998
        public async Task LambdaHandler(S3Event s3Event, ILambdaContext iLambdaContext)
        {
          iAmazonTranscribeService = new AmazonTranscribeServiceClient();
          var objectKey = s3Event.Records[0].S3.Object.Key;
          if (objectKey == null)
          {
            return;
          }

          try
          {
            var transcribeJobName = "Job-";
            var fileLocation = "https://" + _sourceBucket + _prefix + objectKey;
            var startTranscriptionJobRequest = new StartTranscriptionJobRequest()
            {
              TranscriptionJobName = transcribeJobName,
              MediaFormat = MediaFormat.Wav.ToString(),
              Media ={MediaFileUri = fileLocation},
              LanguageCode = LanguageCode.EnGB.ToString(),
              OutputBucketName = _writeBucket
            };
            var startTranscriptionJobResponse = await iAmazonTranscribeService.StartTranscriptionJobAsync(startTranscriptionJobRequest);
            var upper = 1;
            for (var i = 0; i < upper; i++)
            {
              if (startTranscriptionJobResponse.TranscriptionJob.TranscriptionJobStatus.ToString() != "Completed" ||
                  startTranscriptionJobResponse.TranscriptionJob.TranscriptionJobStatus.ToString() != "Failed")
              {

              }
              else
              {
                iLambdaContext.Logger.LogLine($"StartTranscriptionJobResponse : {JsonConvert.SerializeObject(startTranscriptionJobResponse)}\n");
              }
            }
          }
          catch (Exception exception)
          {
            iLambdaContext.Logger.LogLine($"Exception : {JsonConvert.SerializeObject(exception)}\n");
            throw;
          }
        }
    }
}
