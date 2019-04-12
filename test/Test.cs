using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Moq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace My.MySampleModule.PollyFunction.Test {

	public class Test {

		[Fact]
		public async Task CanSendToPolly() {
			//arrange
			var mock = MockLogic.Create();
			mock.PollyMock.Setup(x => x.SynthesizeSpeechAsync(It.IsAny<SynthesizeSpeechRequest>(), It.IsAny<CancellationToken>()))
						.Returns(Task.FromResult(new SynthesizeSpeechResponse() { HttpStatusCode = (HttpStatusCode)200 }));

			mock.S3Mock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new PutObjectResponse() {
				HttpStatusCode = (HttpStatusCode)200
			});

			//act
			await mock.Logic.ProcessRequest("foo", "bar");

			//assert
			mock.PollyMock.VerifyAll();
		}

		
		internal class MockLogic {
			public Mock<IAmazonPolly> PollyMock { get; }
			public Mock<IAmazonS3> S3Mock { get; }
			public Logic Logic { get; }

			public MockLogic() {
				PollyMock = new Mock<IAmazonPolly>();
				S3Mock = new Mock<IAmazonS3>();
				//ILogicDependencyProvider
				//Logic = new Logic(PollyMock.Object, S3Mock.Object, "fake_bucket_name");
			}

			public static MockLogic Create() {
				return new MockLogic();
			}
		}
	}
}