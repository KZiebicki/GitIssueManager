using System.Text.Json;
using System.Text;
using GitIssueManager.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using GitIssueManager.Core.Models;
using GitIssueManager.Core.Helpers;
using Microsoft.Extensions.Configuration;

namespace GitIssueManager.Core.Services
{
    public class GitLabIssueService : IIssueService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public GitLabIssueService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClient.ApplyAuthHeaders(_httpContextAccessor.HttpContext);
        }

        public async Task<HttpResponseMessage> CreateIssueAsync(IssueRequestModel requestModel)
        {
            string url = $"{GetBaseUrl()}/projects/{Uri.EscapeDataString(requestModel.Repository)}/issues";

            var content = new
            {
                title = requestModel.Title,
                description = requestModel.Description
            };

            return await _httpClient.PostAsync(url, AsJson(content));
        }

        public async Task<HttpResponseMessage> UpdateIssueAsync(int issueNumber, IssueRequestModel requestModel)
        {
            string url = $"{GetBaseUrl()}/projects/{Uri.EscapeDataString(requestModel.Repository)}/issues/{issueNumber}";

            var content = new
            {
                title = requestModel.Title,
                description = requestModel.Description
            };

            return await _httpClient.PutAsync(url, AsJson(content));
        }

        public async Task<HttpResponseMessage> CloseIssueAsync(int issueNumber, IssueRequestModel requestModel)
        {
            string url = $"{GetBaseUrl()}/projects/{Uri.EscapeDataString(requestModel.Repository)}/issues/{issueNumber}";

            var content = new
            {
                state_event = "close"
            };

            return await _httpClient.PutAsync(url, AsJson(content));
        }

        private static StringContent AsJson(object obj) =>
            new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        private string GetBaseUrl() =>
            _configuration.GetSection("Providers:GitLab:BaseURL").Value ?? "https://gitlab.com/api/v4";
    }
}
