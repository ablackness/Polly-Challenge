using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LambdaSharpChallenge.PollyToS3Module {

	public interface ILogicDependencyProvider {
		IAmazonPolly Polly { get; set; }
		IAmazonS3 S3 { get; set; }
		IAmazonSimpleNotificationService SnsClient { get; set; }
		string Bucket { get; set; }
		string NotificationTopic { get; set; }

		//--- Methods ---
		Exception AbortNotFound(string message);
	}

	public class ConvertTextRequest {

		//--- Properties ---
		// [JsonRequired]
		// public string Content { get; set; }

		// [JsonRequired]
		// public string FileName { get; set; }

		[JsonRequired]
		public string Language { get; set; }

		[JsonRequired]
		public string Uri { get; set; }
	}

	public class ConvertTextResponse {

		//--- Properties ---
		[JsonRequired]
		public string FileName { get; set; }

		[JsonRequired]
		public string Voice { get; set; }
	}

	public class Logic {

		//--- Fields ---
		private ILogicDependencyProvider _provider;

		//--- Constructors ---
		public Logic(ILogicDependencyProvider provider) {
			_provider = provider ?? throw new ArgumentNullException(nameof(provider));
		}

		public async Task<ConvertTextResponse> AddItem(ConvertTextRequest request) {
			HttpClient httpClient = new HttpClient();
			// string uri = "https://api.rss2json.com/v1/api.json?rss_url=https%3A%2F%2Ftechcrunch.com%2Ffeed%2F";
			var response = await httpClient.GetAsync(request.Uri);
			string responseContent = await response.Content.ReadAsStringAsync();
			dynamic JsonResponse = JObject.Parse(responseContent);
			dynamic items = JsonResponse.items;

			var item = items[0];
			string guid = item.guid;
			int index = guid.IndexOf("?p=");
			string itemId = guid.Substring(index + 3, 7);

			var voicesResponse = _provider.Polly.DescribeVoicesAsync(new DescribeVoicesRequest 
			{
				LanguageCode = request.Language
			});

			Random random = new Random();
			int r = random.Next(voicesResponse.Result.Voices.Count);
			string voice = voicesResponse.Result.Voices[r].Id;

			var pollyRequest = new SynthesizeSpeechRequest {
				OutputFormat = OutputFormat.Mp3,
				Text = item.description,
				TextType = "text",
				VoiceId = voice
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
				Key = itemId + ".mp3",
				InputStream = memoryStream
			});
			if (s3Response == null
				|| s3Response.HttpStatusCode != (HttpStatusCode)200) {
				throw new Exception("Unable to save audio file to s3");
			}
			
			await _provider.SnsClient.PublishAsync(new PublishRequest {
                TopicArn = _provider.NotificationTopic,
                Message = "The audio file is ready: https://s3.amazonaws.com/" + _provider.Bucket + "/" + itemId + ".mp3",
				Subject = item.title
            });

			return new ConvertTextResponse {
				FileName = itemId + ".mp3",
				Voice = voice
			};
		}
	}
}