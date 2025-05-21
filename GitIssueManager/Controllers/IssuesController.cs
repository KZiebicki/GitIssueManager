using GitIssueManager.Core.Models;
using Microsoft.AspNetCore.Mvc;
using GitIssueManager.Core.Factories.Interfaces;

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
            //TODO: ERROR HANDLING!
            await _issueFactory.GetService(request.Provider).CreateIssueAsync(request);
            return Ok("Issue created."); //TODO: IMPROVE THE RESPONSE
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateIssue(int id, [FromBody] IssueRequestModel request)
        {
            await _issueFactory.GetService(request.Provider).UpdateIssueAsync(id, request);
            return Ok("Issue updated."); //TODO: IMPROVE THE RESPONSE
        }

        [HttpPatch("{id:int}/close")]
        public async Task<IActionResult> CloseIssue(int id, [FromBody] IssueRequestModel request)
        {
            await _issueFactory.GetService(request.Provider).CloseIssueAsync(id, request);
            return Ok("Issue closed."); //TODO: IMPROVE THE RESPONSE
        }
    }
}
