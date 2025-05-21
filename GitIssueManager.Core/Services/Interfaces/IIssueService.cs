using GitIssueManager.Core.Models;

namespace GitIssueManager.Core.Services.Interfaces
{
    public interface IIssueService
    {
        Task<HttpResponseMessage> CreateIssueAsync(IssueRequestModel requestModel);
        Task<HttpResponseMessage> UpdateIssueAsync(int issueNumber, IssueRequestModel requestModel);
        Task<HttpResponseMessage> CloseIssueAsync(int issueNumber, IssueRequestModel requestModel);
    }
}
