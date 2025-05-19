namespace GitIssueManager.Core.Services.Interfaces
{
    interface IIsueService
    {
        Task CreateIssue(string repo, string title, string description);
        Task UpdateIssue(string repo, int issueNumber, string title, string description);
        Task CloseIssue(string repo, int issueNumber);
    }
}
