using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LambdaSharpChallenge.PollyToS3Module {

	public interface ILogicDependencyProvider {
		IAmazonPolly Polly { get; set; }
		IAmazonS3 S3 { get; set; }
		// TODO Level 2 Add SNS Client
		string Bucket { get; set; }

		//--- Methods ---
		Exception AbortNotFound(string message);
	}

	public class ConvertTextRequest {

		//--- Properties ---
		[JsonRequired]
		public string Content { get; set; }

		[JsonRequired]
		public string FileName { get; set; }
	}

	public class ConvertTextResponse {

		//--- Properties ---
		[JsonRequired]
		public string FileName { get; set; }
	}

	public class Logic {

		//--- Fields ---
		private ILogicDependencyProvider _provider;

		//--- Constructors ---
		public Logic(ILogicDependencyProvider provider) {
			_provider = provider ?? throw new ArgumentNullException(nameof(provider));
		}

		public async Task<ConvertTextResponse> AddItem(ConvertTextRequest request) {
			var pollyRequest = new SynthesizeSpeechRequest {
				OutputFormat = OutputFormat.Mp3,
				Text = request.Content,
				TextType = "text",
				VoiceId = VoiceId.Amy 
			};

			var pollyResponse = await _provider.Polly.SynthesizeSpeechAsync(pollyRequest);
			if (pollyResponse == null
				|| pollyResponse.HttpStatusCode != (HttpStatusCode)200) {
				throw new Exception("Text could not be converted to audio.");
			}
			var memoryStream = new MemoryStream();
			pollyResponse.AudioStream.CopyTo(memoryStream);
			var s3Response = await _provider.S3.PutObjectAsync(new PutObjectRequest {
				BucketName = _provider.Bucket,
				Key = request.FileName,
				InputStream = memoryStream
			});
			if (s3Response == null
				|| s3Response.HttpStatusCode != (HttpStatusCode)200) {
				throw new Exception("Unable to save audio file to s3");
			}

			// TODO LVL2 SNS Topic would be nice here
			return new ConvertTextResponse {
				FileName = request.FileName
			};
		}
	}
}