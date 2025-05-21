using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace GitIssueManager.Core.Helpers
{
    public static class HttpClientAuthHelper
    {
        public static void ApplyAuthHeaders(this HttpClient client, HttpContext? context)
        {
            var authHeader = context?.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader))
            {
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GitIssueManager", "1.0"));
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authHeader.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
