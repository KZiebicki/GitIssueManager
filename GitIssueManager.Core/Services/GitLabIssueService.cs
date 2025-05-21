using System.Text.Json;
using System.Text;
using GitIssueManager.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using GitIssueManager.Core.Models;
using GitIssueManager.Core.Helpers;

namespace GitIssueManager.Core.Services
{
    public class GitLabIssueService : IIssueService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GitLabIssueService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _httpClient.ApplyAuthHeaders(_httpContextAccessor.HttpContext);
        }

        public async Task<HttpResponseMessage> CreateIssueAsync(IssueRequestModel requestModel)
        {
            string url = $"https://gitlab.com/api/v4/projects/{Uri.EscapeDataString(requestModel.Repository)}/issues";

            var content = new
            {
                title = requestModel.Title,
                description = requestModel.Description
            };

            return await _httpClient.PostAsync(url, AsJson(content));
        }

        public async Task<HttpResponseMessage> UpdateIssueAsync(int issueNumber, IssueRequestModel requestModel)
        {
            string url = $"https://gitlab.com/api/v4/projects/{Uri.EscapeDataString(requestModel.Repository)}/issues/{issueNumber}";

            var content = new
            {
                title = requestModel.Title,
                description = requestModel.Description
            };

            return await _httpClient.PutAsync(url, AsJson(content));
        }

        public async Task<HttpResponseMessage> CloseIssueAsync(int issueNumber, IssueRequestModel requestModel)
        {
            string url = $"https://gitlab.com/api/v4/projects/{Uri.EscapeDataString(requestModel.Repository)}/issues/{issueNumber}";

            var content = new
            {
                state_event = "close"
            };

            return await _httpClient.PutAsync(url, AsJson(content));
        }

        private static StringContent AsJson(object obj) =>
            new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }
}
