using GitIssueManager.Core.Services.Interfaces;

namespace GitIssueManager.Core.Services
{
    public class GitHubIssueService : IIsueService
    {
        public Task CloseIssue(string repo, int issueNumber)
        {
            throw new NotImplementedException();
        }

        public Task CreateIssue(string repo, string title, string description)
        {
            throw new NotImplementedException();
        }

        public Task UpdateIssue(string repo, int issueNumber, string title, string description)
        {
            throw new NotImplementedException();
        }
    }
}
