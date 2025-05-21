using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using GitIssueManager.Core.Services.Interfaces;

namespace GitIssueManager.Core.Services
{
    public class GitLabIssueService : IIssueService
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;
        private readonly string _baseUrl;

        public GitLabIssueService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _token = "";
            _baseUrl = "https://gitlab.com/api/v4";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task CreateIssueAsync(string repo, string title, string description)
        {
            //TODO: CREATE GITLAB ACCOUNT AND TEST THIS STUFF
            var url = $"{_baseUrl}/projects/{Uri.EscapeDataString(repo)}/issues";

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
            var url = $"{_baseUrl}/projects/{Uri.EscapeDataString(repo)}/issues/{issueNumber}";

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
            var url = $"{_baseUrl}/projects/{Uri.EscapeDataString(repo)}/issues/{issueNumber}";

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
