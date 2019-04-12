using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace My.MySampleModule.PollyFunction {

	public class Logic {
		private readonly IAmazonPolly _polly;
		private readonly IAmazonS3 _s3;
		private readonly string _bucket;
		public readonly CancellationToken CancelToken;

		public Logic(IAmazonPolly polly, IAmazonS3 s3, string bucket) {
			_polly = polly;
			_s3 = s3;
			_bucket = bucket;
		}

		public async Task ProcessRequest(string content, string title) {
			var pollyRequest = new SynthesizeSpeechRequest {
				Text = content,
				OutputFormat = OutputFormat.Mp3,
				VoiceId = VoiceId.Amy
			};
			var pollyResponse = await _polly.SynthesizeSpeechAsync(pollyRequest);
			if (pollyResponse == null
				|| pollyResponse.HttpStatusCode != (HttpStatusCode)200) {
				throw new Exception("Text could not be converted to audio.");
			}
			var s3Response = await _s3.PutObjectAsync(new PutObjectRequest {
				BucketName = _bucket,
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