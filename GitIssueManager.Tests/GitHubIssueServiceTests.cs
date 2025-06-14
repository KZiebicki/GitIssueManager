﻿using System.Net;
using System.Net.Http.Headers;
using System.Text;
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
        private readonly Mock<HttpMessageHandler> _httpHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly GitHubIssueService _service;

        public GitHubIssueServiceTests()
        {
            _httpHandlerMock = new Mock<HttpMessageHandler>();

            _httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            _httpClient = new HttpClient(_httpHandlerMock.Object);

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = "Bearer dummy-token";
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            _configuration = new ConfigurationBuilder().Build();

            _service = new GitHubIssueService(_httpClient, _httpContextAccessor.Object, _configuration);
        }

        [Fact]
        public async Task CreateIssueAsync_GitHubSuccess()
        {
            var request = new IssueRequestModel
            {
                Provider = IssueProvider.GitHub,
                Repository = "user/repo",
                Title = "New Issue",
                Description = "Issue description"
            };

            await _service.CreateIssueAsync(request);

            _httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.ToString() == "https://api.github.com/repos/user/repo/issues" &&
                    req.Headers.Authorization!.Scheme == "Bearer" &&
                    req.Headers.Authorization.Parameter == "dummy-token" &&
                    req.Content!.Headers.ContentType!.MediaType == "application/json"
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task UpdateIssueAsync_GitHubSuccess()
        {
            var request = new IssueRequestModel
            {
                Provider = IssueProvider.GitHub,
                Repository = "user/repo",
                Title = "Updated Title",
                Description = "Updated description"
            };

            await _service.UpdateIssueAsync(42, request);

            _httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Patch &&
                    req.RequestUri!.ToString().EndsWith("/repos/user/repo/issues/42")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task CloseIssueAsync_GitHubSuccess()
        {
            var request = new IssueRequestModel
            {
                Provider = IssueProvider.GitHub,
                Repository = "user/repo"
            };

            await _service.CloseIssueAsync(13, request);

            _httpHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Patch &&
                    req.RequestUri!.ToString().EndsWith("/repos/user/repo/issues/13")
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
