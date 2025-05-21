using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using GitIssueManager.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;

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

            //TODO: MOVE TO SOME HELPER CLASS PROBABLY
            string? authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader))
            {
                _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitIssueManager", "1.0"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", ""));//TODO: Bearer is case sensitive. Need fix
            }
        }

        public async Task CreateIssueAsync(string repo, string title, string description)
        {
            var url = $"https://gitlab.com/api/v4/projects/{Uri.EscapeDataString(repo)}/issues";

            var content = new
            {
                title,
                description
            };

            var response = await _httpClient.PostAsync(url, AsJson(content));
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string repo, int issueNumber, string title, string description)
        {
            var url = $"https://gitlab.com/api/v4/projects/{Uri.EscapeDataString(repo)}/issues/{issueNumber}";

            var content = new
            {
                title,
                description
            };

            var response = await _httpClient.PutAsync(url, AsJson(content));
            response.EnsureSuccessStatusCode();
        }

        public async Task CloseIssueAsync(string repo, int issueNumber)
        {
            var url = $"https://gitlab.com/api/v4/projects/{Uri.EscapeDataString(repo)}/issues/{issueNumber}";

            var content = new
            {
                state_event = "close"
            };

            var response = await _httpClient.PutAsync(url, AsJson(content));
            response.EnsureSuccessStatusCode();
        }

        private static StringContent AsJson(object obj) =>
            new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }
}
