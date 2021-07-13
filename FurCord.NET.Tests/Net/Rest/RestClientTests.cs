using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace FurCord.NET.Tests.Net.Rest
{
	public class RestClientTests
	{
		private Mock<FakeHttpMessageHandler> _httpMessageHandler;
		
		private class FakeHttpMessageHandler : HttpMessageHandler
		{
			public virtual HttpResponseMessage Send(HttpRequestMessage request) => throw new NotImplementedException();

			protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) 
				=> Task.FromResult(Send(request));
		}
		
		[OneTimeSetUp]
		public void SetUp()
		{
			_httpMessageHandler = new() {CallBase = true};
		}

		[Test]
		public async Task RestClient_SendRequest_SetsResult()
		{
			//Arrange
			var content = "{\"content\":\"this content is pretty pog!\"}"; 
			
			_httpMessageHandler
				.Setup(f => f.Send(It.IsAny<HttpRequestMessage>()))
				.Returns(new HttpResponseMessage()
				{
					Content = new StringContent(content),
					StatusCode = HttpStatusCode.OK
				});
			
			var client = new RestClient("", _httpMessageHandler.Object);
			var request = new RestRequest("", RestMethod.GET);
			
			//Act
			await client.DoRequestAsync(request);
			
			//Assert
			Assert.True(request.Response.IsCompleted);
			Assert.AreEqual(request.Response.Result.Content, content);
		}
		
		[Test]
		public async Task RestClient_SendRequest_ReplacesParameterizedRequest()
		{
			//Arrange
			var content = "{\"content\":\"this content is pretty pog!\"}";
			HttpRequestMessage reqMessage = null;
			
			_httpMessageHandler
				.Setup(f => f.Send(It.IsAny<HttpRequestMessage>()))
				.Callback<HttpRequestMessage>(m => reqMessage = m)
				.Returns(new HttpResponseMessage()
				{
					Content = new StringContent(content),
					StatusCode = HttpStatusCode.OK
				});
			
			var client = new RestClient("", _httpMessageHandler.Object);
			
			var request = new RestRequest("channels/:channel_id", RestMethod.GET, new() { ["channel_id"] = 0});
			
			//Act
			await client.DoRequestAsync(request);
			
			//Assert
			Assert.AreEqual("https://discord.com/api/v9/channels/0", reqMessage.RequestUri!.ToString());
		}

		[Test]
		public async Task RestClient_RequeuesRequest_WhenRatelimited()
		{
			bool isComplete;
			
			//Arrange
			var wait = (DateTimeOffset.UtcNow.AddSeconds(2).Subtract(DateTimeOffset.UnixEpoch));
			var initialRateLimitHeaderMessage = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
			initialRateLimitHeaderMessage.Headers.Add("X-RateLimit-Remaining", "0");
			initialRateLimitHeaderMessage.Headers.Add("X-RateLimit-Limit", "1");
			initialRateLimitHeaderMessage.Headers.Add("X-RateLimit-Reset", wait.TotalSeconds.ToString("F0"));
			initialRateLimitHeaderMessage.Headers.Add("X-RateLimit-Bucket", "0fwyaf87");

			var wait2 = (DateTimeOffset.UtcNow.AddSeconds(4).Subtract(DateTimeOffset.UnixEpoch));
			var okayRateLimitHeaderMessage = new HttpResponseMessage(HttpStatusCode.OK);
			okayRateLimitHeaderMessage.Headers.Add("X-RateLimit-Remaining", "1");
			okayRateLimitHeaderMessage.Headers.Add("X-RateLimit-Limit", "1");
			okayRateLimitHeaderMessage.Headers.Add("X-RateLimit-Reset", wait2.TotalSeconds.ToString("F0"));
			okayRateLimitHeaderMessage.Headers.Add("X-RateLimit-Bucket", "0fwyaf87");
			
		_httpMessageHandler
			.SetupSequence(f => f.Send(It.IsAny<HttpRequestMessage>()))
			.Returns(initialRateLimitHeaderMessage)
			.Returns(okayRateLimitHeaderMessage);
			
			var client = new RestClient("", _httpMessageHandler.Object);
			var request = new RestRequest("", RestMethod.GET);
			
			//Act
			await client.DoRequestAsync(request); // This is necessary because it doesn't actually check the response. I'll fix that soon™ //
			await client.DoRequestAsync(request);
			isComplete = request.Response.IsCompleted;
			await request.Response;
			
			//Assert
			Assert.False(isComplete);
			Assert.True(request.Response.IsCompleted);
		}
		
	}
}