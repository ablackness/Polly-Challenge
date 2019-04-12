using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace My.MySampleModule.PollyFunction {

	public interface ILogicDependencyProvider {
		IAmazonPolly Polly { get; set; }
		IAmazonS3 S3 { get; set; }
		string Bucket { get; set; }

		//--- Methods ---
		Exception AbortNotFound(string message);
	}

	public class AddItemRequest {

		//--- Properties ---
		[JsonRequired]
		public string Content { get; set; }

		[JsonRequired]
		public string Title { get; set; }
	}

	public class AddItemResponse {

		//--- Properties ---
		[JsonRequired]
		public string Id { get; set; }
	}

	public class Logic {

		//--- Fields ---
		private ILogicDependencyProvider _provider;

		//--- Constructors ---
		public Logic(ILogicDependencyProvider provider) {
			_provider = provider ?? throw new ArgumentNullException(nameof(provider));
		}

		public async Task<AddItemResponse> AddItem(AddItemRequest request) {
			Console.Write("Content: "+ request.Content);
			await ProcessRequest(request.Content, request.Title);
			return new AddItemResponse {
				Id = "Ok"
			};
		}

		public async Task ProcessRequest(string content, string title) {
			var pollyRequest = new SynthesizeSpeechRequest {
				Text = content,
				OutputFormat = OutputFormat.Mp3,
				VoiceId = VoiceId.Amy
			};

			var pollyResponse = await _provider.Polly.SynthesizeSpeechAsync(pollyRequest);
			if (pollyResponse == null
				|| pollyResponse.HttpStatusCode != (HttpStatusCode)200) {
				throw new Exception("Text could not be converted to audio.");
			}
			pollyResponse.AudioStream.Position = 0;
			var s3Response = await _provider.S3.PutObjectAsync(new PutObjectRequest {
				BucketName = _provider.Bucket,
				Key = title,
				InputStream = pollyResponse.AudioStream
			});
			if (s3Response == null
				|| s3Response.HttpStatusCode != (HttpStatusCode)200) {
				throw new Exception("Unable to save audio file to s3");
			}
		}
	}
}