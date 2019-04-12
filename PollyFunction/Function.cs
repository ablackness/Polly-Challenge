using Amazon.Lambda.Core;
using Amazon.Polly;
using Amazon.S3;
using LambdaSharp;
using LambdaSharp.APIGateway;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace My.MySampleModule.PollyFunction {

	public class Function : ALambdaAPIGatewayFunction, ILogicDependencyProvider {
		public IAmazonPolly Polly { get; set; }
		public IAmazonS3 S3 { get; set; }
		public string Bucket { get; set; }

		//--- Methods ---
		public override Task InitializeAsync(LambdaConfig config) {
			
			Polly = new AmazonPollyClient();
			S3 = new AmazonS3Client();
			Bucket = config.ReadS3BucketName("ArticlesBucket");
			return Task.CompletedTask;
		}

		//--- ILogicDependencyProvider Members ---
		Exception ILogicDependencyProvider.AbortNotFound(string message) => AbortNotFound(message);
	}
}