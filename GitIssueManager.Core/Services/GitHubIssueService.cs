using System.Text.Json;
using System.Text;
using GitIssueManager.Core.Services.Interfaces;
using System.Net.Http.Headers;

namespace GitIssueManager.Core.Services
{
    public class GitHubIssueService : IIssueService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public GitHubIssueService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //TODO: TOKEN AUTH
        }

        public async Task CreateIssueAsync(string repo, string title, string description) //TODO: CHANGE TO IssueRequestModel
        {
            //var authHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            //if (!string.IsNullOrEmpty(authHeader))
            //{
            //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", ""));
            //}


            var url = $"https://api.github.com/repos/{repo}/issues";

            var content = new
            {
                title,
                body = description
            };

            var response = await _httpClient.PostAsync(url, AsJson(content));
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateIssueAsync(string repo, int issueNumber, string title, string description)
        {
            var url = $"https://api.github.com/repos/{repo}/issues/{issueNumber}";

            var content = new
            {
                title,
                body = description
            };

            var response = await _httpClient.PatchAsync(url, AsJson(content));
            response.EnsureSuccessStatusCode();
        }

        public async Task CloseIssueAsync(string repo, int issueNumber)
        {
            var url = $"https://api.github.com/repos/{repo}/issues/{issueNumber}";

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
