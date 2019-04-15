# Amazon Polly Challenge

In this challenge, we'll be exploring Amazon Polly to convert text to audio.

## Helpful Links

[.NET AWS Polly SDK Docs](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html?page=Polly/MPollySynthesizeSpeechSynthesizeSpeechRequest.html&tocid=Amazon_Polly_AmazonPollyClient)
[.NET AWS SDK Docs](https://docs.aws.amazon.com/sdkfornet/v3/apidocs)

## Pre-requisites

The following tools and accounts are required to complete these instructions.

- [AWS Account](https://aws.amazon.com/)
- [Install AWS CLI](https://aws.amazon.com/cli/)
- [Install .NET Core 2.2](https://www.microsoft.com/net/download)
- [LambdaSharp Tool](https://github.com/LambdaSharp/LambdaSharpTool) - This is important! `dotnet tool install --global LambdaSharp.Tool --version 0.6.0-RC1`

## Level 0

- Clone this repo.
- `lash deploy --tier Challenge`. This will build and deploy your lambda function.
- Test it by getting the API Gateway unique url in the lambda function: `curl -d '{"Content": "Hello world! This is some test content.", "Title": "Hello World!"}' -H "Content-Type: application/json" -X POST https:/REPLACEME.execute-api.us-east-1.amazonaws.com/LATEST/articles`

## Level 1

After the text is converted to audio, we want to be able to access anytime. Make the S3 bucket public. It's defined in `Module.yml`.
See this LambdaSharp example: https://github.com/LambdaSharp/StaticWebsite-Sample

## Level 2

Let's be notified when audio file is ready. Send a SMS using a SNS topic.

Hint: [Amazon SNS Publish](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html?page=SNS/MSNSPublishAsyncStringStringCancellationToken.html&tocid=Amazon_SimpleNotificationService_Amaz)

Hint 2: Create a SNS topic in `Module.yml`

## Level 3

## Level 4

## Boss
