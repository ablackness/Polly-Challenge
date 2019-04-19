using Amazon.Lambda.Core;
using Amazon.Polly;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using LambdaSharp;
using LambdaSharp.APIGateway;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaSharpChallenge.PollyToS3Module {

	public class Function : ALambdaAPIGatewayFunction, ILogicDependencyProvider {
		public IAmazonPolly Polly { get; set; }
		public IAmazonS3 S3 { get; set; }
		public IAmazonSimpleNotificationService SnsClient { get; set; }
		public string Bucket { get; set; }
		public string NotificationTopic { get; set; }

		//--- Methods ---
		public override Task InitializeAsync(LambdaConfig config) {
			Polly = new AmazonPollyClient();
			S3 = new AmazonS3Client();
			SnsClient = new AmazonSimpleNotificationServiceClient();
			Bucket = config.ReadS3BucketName("ArticlesBucket");
			NotificationTopic = config.ReadText("ArticleAudioDone");
			return Task.CompletedTask;
		}

		//--- ILogicDependencyProvider Members ---
		Exception ILogicDependencyProvider.AbortNotFound(string message) => AbortNotFound(message);
	}
}