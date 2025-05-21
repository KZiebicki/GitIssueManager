using GitIssueManager.Core.Models;

namespace GitIssueManager.Core.Services.Interfaces
{
    public interface IIssueService
    {
        Task CreateIssueAsync(IssueRequestModel requestModel);
        Task UpdateIssueAsync(int issueNumber, IssueRequestModel requestModel);
        Task CloseIssueAsync(int issueNumber, IssueRequestModel requestModel);
    }
}
