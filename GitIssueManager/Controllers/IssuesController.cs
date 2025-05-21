using GitIssueManager.Core.Models;
using Microsoft.AspNetCore.Mvc;
using GitIssueManager.Core.Factories.Interfaces;
using Newtonsoft.Json;

namespace GitIssueManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueServiceFactory _issueFactory;

        public IssuesController(IIssueServiceFactory issueFactory)
        {
            _issueFactory = issueFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromBody] IssueRequestModel request)
        {
            try
            {
                var response = await _issueFactory.GetService(request.Provider).CreateIssueAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, FormatJson(responseBody));
            }
            catch (NotSupportedException)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, $"Provider: {request.Provider} is not supported");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error");
            }
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateIssue(int id, [FromBody] IssueRequestModel request)
        {
            try
            {
                var response = await _issueFactory.GetService(request.Provider).UpdateIssueAsync(id, request);
                string responseBody = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, FormatJson(responseBody));
            }
            catch (NotSupportedException)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, $"Provider: {request.Provider} is not supported");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error");
            }
        }

        [HttpPatch("{id:int}/close")]
        public async Task<IActionResult> CloseIssue(int id, [FromBody] IssueRequestModel request)
        {
            try
            {
                var response = await _issueFactory.GetService(request.Provider).CloseIssueAsync(id, request);
                string responseBody = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, FormatJson(responseBody));
            }
            catch (NotSupportedException)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, $"Provider: {request.Provider} is not supported");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error");
            }
        }

        private static string FormatJson(string json)
        {
            try
            {
                var parsedJson = JsonConvert.DeserializeObject(json);
                return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }
            catch (Exception)
            {
                return json;
            }
        }
    }
}
