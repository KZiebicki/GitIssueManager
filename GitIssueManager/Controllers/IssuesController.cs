using GitIssueManager.Core.Models;
using Microsoft.AspNetCore.Mvc;
using GitIssueManager.Core.Services.Interfaces;
using GitIssueManager.Core.Services;
using Microsoft.AspNetCore.Authorization;

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
            await _issueFactory.GetService(request.Provider).CreateIssueAsync(request.Repository, request.Title, request.Description);
            return Ok("Issue created."); //TODO: IMPROVE THE RESPONSE
        }

        [HttpPut("{id:int}")] //TODO: Check the verb. PUT For updating?
        public async Task<IActionResult> UpdateIssue(int id, [FromBody] IssueRequestModel request)
        {
            await _issueFactory.GetService(request.Provider).UpdateIssueAsync(request.Repository, id, request.Title, request.Description);
            return Ok("Issue updated."); //TODO: IMPROVE THE RESPONSE
        }

        [HttpPatch("{id:int}/close")] //TODO: Check the verb. PATCH For closing?
        public async Task<IActionResult> CloseIssue(int id, [FromBody] IssueRequestModel request)
        {
            await _issueFactory.GetService(request.Provider).CloseIssueAsync(request.Repository, id);
            return Ok("Issue closed."); //TODO: IMPROVE THE RESPONSE
        }
    }
}
