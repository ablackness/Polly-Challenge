# Amazon Polly Challenge

In this challenge, we'll be exploring Amazon Polly to convert text to audio.

![Flow](case-study.png)

## Helpful Links

[.NET AWS Polly SDK Docs](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html?page=Polly/MPollySynthesizeSpeechSynthesizeSpeechRequest.html&tocid=Amazon_Polly_AmazonPollyClient)
[.NET AWS SDK Docs](https://docs.aws.amazon.com/sdkfornet/v3/apidocs)

## Pre-requisites

The following tools and accounts are required to complete these instructions.

- [AWS Account](https://aws.amazon.com/)
- [GitHub Account](https://github.com/)
- [Install AWS CLI](https://aws.amazon.com/cli/)
- [Install .NET Core 2.2](https://www.microsoft.com/net/download)
- [LambdaSharp Tool](https://github.com/LambdaSharp/LambdaSharpTool) - This is important! `dotnet tool install --global LambdaSharp.Tool --version 0.6.0-RC1`

## Level 0

- Clone this repo.
- `lash deploy --tier Challenge`. This will build and deploy your lambda function.
- Get the API Gateway url from the lambda function created by Lambda#.
- Test the API Gateway endpoint like this: `curl -d '{"Content": "Hello world! This is some test content.", "FileName": "test.mp3"}' -H "Content-Type: application/json" -X POST https:/REPLACEME.execute-api.us-east-1.amazonaws.com/LATEST/articles` Works with Git Bash and *Nix

## Level 1

After the text is converted to audio, we want to be able to access the files anytime. Make the S3 bucket public. It's defined in `Module.yml`.

See this LambdaSharp example: https://github.com/LambdaSharp/StaticWebsite-Sample

Don't forget to deploy the updated stack! `lash deploy --tier Challenge`. You will need to do this anytime you make a change.

<details><summary>Not hard enough for you?</summary>
We don't want to expend processing power on duplicate files! If the content of the incoming article is identical to one that has already been saved, then ignore it.
</details>

## Level 2

Let's be notified when the audio file is ready. Send an SMS/Email using an SNS topic. Add a link to the mp3 in the SNS notification.

<details><summary>Hint 1</summary>

[Amazon SNS Publish](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html?page=SNS/MSNSPublishAsyncStringStringCancellationToken.html&tocid=Amazon_SimpleNotificationService_Amaz)

</details>

<details><summary>Hint 2</summary>
Create a SNS topic in `Module.yml` using `lash new resource ArticleAudioDone AWS::SNS::Topic`
</details>

## Level 3

We want to poll an article list with title and description every 5 minutes from an RSS feed. Typically this would be done every day but for the purposes of this challenge, 5 minute intervals will work. Save the audio file in the following format `YYMMDDHHMMSS.mp3`

Example RSS Feed 1: https://hnrss.org/newest

Example RSS Feed 2: https://www.reddit.com/r/news/.rss

Like JSON better? Check this out https://rss2json.com/#rss_url=https%3A%2F%2Ftechcrunch.com%2Ffeed%2F

Check out these docs for some details on scheduling with AWS
https://docs.aws.amazon.com/lambda/latest/dg/tutorial-scheduled-events-schedule-expressions.html
https://docs.aws.amazon.com/AmazonCloudWatch/latest/events/RunLambdaSchedule.html

<details><summary>Not hard enough for you?</summary>
Parse the article's html into plain text then convert it to an mp3.  This could be in the field: `content:encoded`
</details>

## Level 4

We want the user to be able to choose a language and get audio based on that selection. 

Use Amazon Polly's built in localization methods to describe the voices available for that language code. 

Pick a voice from that list and use it to synthesize the text to speech in the chosen language.

NOTE: This should not require any text translation.

## Boss

We want to be able to listen to the audio in the language of our choice. Use Amazon Transcribe to translate the text into another language before sending it to Polly.

<details><summary>Hint 1</summary>
Polly and Transcribe are similar services. Use the existing definitions in the `Module.yml` for ideas.
</details>

<details><summary>Hint 2</summary>
No, this is the boss level!
</details>
