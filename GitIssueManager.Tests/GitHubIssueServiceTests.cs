using System.Net;
using GitIssueManager.Core.Models;
using GitIssueManager.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace GitIssueManager.Tests
{
    public class GitHubIssueServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly IConfigurationRoot _configuration;

        public GitHubIssueServiceTests()
        {
            //TODO: THIS TEST IS NOT LOOKING AS GOOD AS I WANTED. SHOULD PROBABLY BE REWRITTEN
            //TODO: WRITE MORE TESTS
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = "Bearer dummy-token";
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);

            _configuration = new ConfigurationBuilder().Build();
        }

        [Fact]
        public async Task CreateIssueAsync_SendsCorrectRequest()
        {
            // Arrange
            var service = new GitHubIssueService(_httpClient, _httpContextAccessorMock.Object, _configuration);

            var request = new IssueRequestModel
            {
                Provider = IssueProvider.GitHub,
                Repository = "testuser/testrepo",
                Title = "Test Issue",
                Description = "Description here"
            };

            // Act
            await service.CreateIssueAsync(request);

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString() == "https://api.github.com/repos/testuser/testrepo/issues" &&
                    req.Headers.Authorization!.Scheme == "Bearer" &&
                    req.Headers.Authorization.Parameter == "dummy-token"
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
