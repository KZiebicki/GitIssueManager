namespace GitIssueManager.Core.Services.Interfaces
{
    public interface IIssueService
    {
        Task CreateIssueAsync(string repo, string title, string description);
        Task UpdateIssueAsync(string repo, int issueNumber, string title, string description);
        Task CloseIssueAsync(string repo, int issueNumber);
    }
}
