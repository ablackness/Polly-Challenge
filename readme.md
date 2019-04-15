# Amazon Polly Challenge

In this challenge, we'll be exploring Amazon Polly to convert text to audio.

## Helpful Links

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

Test it by getting the API Gateway unique url: `curl -d '{"Content": "test", "Title": "test_1"}' -H "Content-Type: application/json" -X POST https:/REPLACEME.execute-api.us-east-1.amazonaws.com/LATEST/articles`

## Level 1
