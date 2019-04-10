using Amazon.Polly;

namespace My.MySampleModule.PollyFunction {

    public class Logic {
        private readonly IAmazonPolly _polly;
        public Logic(IAmazonPolly polly) {
            _polly = polly;
        }
        public void SendToPolly(string content, string title) {

        }
    }
}