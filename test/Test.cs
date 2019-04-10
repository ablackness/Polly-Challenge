using System.Threading;
using System.Threading.Tasks;
using Amazon.Polly;
using Amazon.Polly.Model;
using Moq;
using Xunit;

namespace My.MySampleModule.PollyFunction.Test {

    public class Test {

        [Fact]
        public void CanInit() {

        }

[Fact]
        public async Task CanSendToPolly() {
            //arrange
            var mock = new Mock<IAmazonPolly>();
            var n = new CancellationToken();
            mock.Setup(x => x.SynthesizeSpeechAsync(It.IsAny<SynthesizeSpeechRequest>(), n )).ReturnsAsync(It.IsAny<SynthesizeSpeechResponse>());
            var logic = new Logic(mock.Object);

            //act
            logic.SendToPolly("foo bar","foo bar title");

            //assert
            // mock.Verify(x => x.SynthesizeSpeechAsync(new SynthesizeSpeechRequest(){
            //     Text = "foo bar"
            // }, n).Result);
            mock.VerifyAll();
        }
    }

}
