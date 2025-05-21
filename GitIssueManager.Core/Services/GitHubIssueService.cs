using System.Text.Json;
using System.Text;
using GitIssueManager.Core.Services.Interfaces;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using GitIssueManager.Core.Models;

namespace GitIssueManager.Core.Services
{
    public class GitHubIssueService : IIssueService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GitHubIssueService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            //TODO: MOVE TO SOME HELPER CLASS PROBABLY
            string? authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader))
            {
                _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitIssueManager", "1.0"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase));
            }
        }

        public async Task CreateIssueAsync(IssueRequestModel requestModel)
        {
            string url = $"https://api.github.com/repos/{requestModel.Repository}/issues";

            var content = new
            {
                title = requestModel.Title,
                body = requestModel.Description
            };

            var response = await _httpClient.PostAsync(url, AsJson(content));
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(int issueNumber, IssueRequestModel requestModel)
        {
            string url = $"https://api.github.com/repos/{requestModel.Repository}/issues/{issueNumber}";

            var content = new
            {
                title = requestModel.Title,
                body = requestModel.Description
            };

            var response = await _httpClient.PatchAsync(url, AsJson(content));
            response.EnsureSuccessStatusCode();
        }

        public async Task CloseIssueAsync(int issueNumber, IssueRequestModel requestModel)
        {
            string url = $"https://api.github.com/repos/{requestModel.Repository}/issues/{issueNumber}";

            var content = new
            {
                state = "closed"
            };

            var response = await _httpClient.PatchAsync(url, AsJson(content));
            response.EnsureSuccessStatusCode();
        }

        private static StringContent AsJson(object obj) =>
            new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }
}
