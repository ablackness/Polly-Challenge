using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using LambdaSharp;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Polly;
using Amazon.Polly.Model;
using System.Collections.Generic;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace My.MySampleModule.PollyFunction {

    public class FunctionRequest {

        //--- Properties ---

        // TODO: add request fields
    }

    public class FunctionResponse {

        //--- Properties ---

        // TODO: add response fields
    }

    public class Function : ALambdaApiGatewayFunction {

        //--- Fields ---
        private AmazonPollyClient _pollyClient;
        //--- Methods ---
        public override async Task InitializeAsync(LambdaConfig config) { }

        public override async Task<APIGatewayProxyResponse> HandleRequestAsync(APIGatewayProxyRequest request, ILambdaContext context) {

            // TODO: add business logic
            LogInfo($"Body = {request.Body}");
            LogDictionary("Headers", request.Headers);
            LogInfo($"HttpMethod = {request.HttpMethod}");
            LogInfo($"IsBase64Encoded = {request.IsBase64Encoded}");
            LogInfo($"Path = {request.Path}");
            LogDictionary("PathParameters", request.PathParameters);
            LogDictionary("QueryStringParameters", request.QueryStringParameters);
            LogInfo($"RequestContext.ResourcePath = {request.RequestContext.ResourcePath}");
            LogInfo($"RequestContext.Stage = {request.RequestContext.Stage}");
            LogInfo($"Resource = {request.Resource}");
            LogDictionary("StageVariables", request.StageVariables);

            if(request.HttpMethod == "POST") {
                // intitiate Polly
                LogInfo("POSTPOSTPOSTPOST");
            }

            return new APIGatewayProxyResponse {
                Body = "Ok",
                Headers = new Dictionary<string, string> {
                    ["Content-Type"] = "text/plain"
                },
                StatusCode = 200
            };

            void LogDictionary(string prefix, IDictionary<string, string> keyValues) {
                if(keyValues != null) {
                    foreach(var keyValue in keyValues) {
                        LogInfo($"{prefix}.{keyValue.Key} = {keyValue.Value}");
                    }
                }
            }
        }
    }

}
