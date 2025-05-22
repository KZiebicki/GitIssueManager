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
        private readonly ILogger<IssuesController> _logger;

        public IssuesController(IIssueServiceFactory issueFactory, ILogger<IssuesController> logger)
        {
            _issueFactory = issueFactory;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIssue([FromBody] IssueRequestModel request)
        {
            _logger.LogInformation("Creating issue. RequestId: {RequestId}, Provider: {Provider}", HttpContext.TraceIdentifier, request.Provider);
            try
            {
                var response = await _issueFactory.GetService(request.Provider).CreateIssueAsync(request);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    _logger.LogWarning("Provider {Provider} responded with error. StatusCode: {StatusCode}", request.Provider, response.StatusCode);
                else
                    _logger.LogInformation("Issue created successfully with provider: {Provider}, StatusCode: {StatusCode}", request.Provider, response.StatusCode);

                return StatusCode((int)response.StatusCode, FormatJson(responseBody));
            }
            catch (NotSupportedException)
            {
                _logger.LogError("Provider not supported: {Provider}", request.Provider);
                return StatusCode(StatusCodes.Status422UnprocessableEntity, $"Provider: {request.Provider} is not supported");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating issue with provider: {Provider}", request.Provider);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateIssue(int id, [FromBody] IssueRequestModel request)
        {
            _logger.LogInformation("Updating issue. RequestId: {RequestId}, Provider: {Provider}, IssueId: {id}", HttpContext.TraceIdentifier, request.Provider, id);
            try
            {
                var response = await _issueFactory.GetService(request.Provider).UpdateIssueAsync(id, request);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    _logger.LogWarning("Provider {Provider} responded with error. StatusCode: {StatusCode}", request.Provider, response.StatusCode);
                else
                    _logger.LogInformation("Issue updated successfully with provider: {Provider}, StatusCode: {StatusCode}", request.Provider, response.StatusCode);

                return StatusCode((int)response.StatusCode, FormatJson(responseBody));
            }
            catch (NotSupportedException)
            {
                _logger.LogError("Provider not supported: {Provider}", request.Provider);
                return StatusCode(StatusCodes.Status422UnprocessableEntity, $"Provider: {request.Provider} is not supported");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating issue with id: {id} with provider: {Provider}", id, request.Provider);
                return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error");
            }
        }

        [HttpPatch("{id:int}/close")]
        public async Task<IActionResult> CloseIssue(int id, [FromBody] IssueRequestModel request)
        {
            _logger.LogInformation("Closing issue. RequestId: {RequestId}, Provider: {Provider}, IssueId: {id}", HttpContext.TraceIdentifier, request.Provider, id);
            try
            {
                var response = await _issueFactory.GetService(request.Provider).CloseIssueAsync(id, request);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    _logger.LogWarning("Provider {Provider} responded with error. StatusCode: {StatusCode}", request.Provider, response.StatusCode);
                else
                    _logger.LogInformation("Issue closed successfully with provider: {Provider}, StatusCode: {StatusCode}", request.Provider, response.StatusCode);

                return StatusCode((int)response.StatusCode, FormatJson(responseBody));
            }
            catch (NotSupportedException)
            {
                _logger.LogError("Provider not supported: {Provider}", request.Provider);
                return StatusCode(StatusCodes.Status422UnprocessableEntity, $"Provider: {request.Provider} is not supported");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing issue with id: {id} with provider: {Provider}", id, request.Provider);
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
