using Microsoft.AspNetCore.Mvc;

namespace GitIssueManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IssuesController : ControllerBase
    {
        [HttpPut(Name = "PutIssues")]
        public int ModifyIssue()
        {
            return 156;
        }
    }
}
