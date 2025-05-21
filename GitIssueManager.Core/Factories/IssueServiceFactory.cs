using GitIssueManager.Core.Factories.Interfaces;
using GitIssueManager.Core.Models;
using GitIssueManager.Core.Services;
using GitIssueManager.Core.Services.Interfaces;

namespace GitIssueManager.Core.Factories
{
    public class IssueServiceFactory : IIssueServiceFactory
    {
        private readonly IServiceProvider _provider;

        public IssueServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IIssueService GetService(IssueProvider provider)
        {
            return provider switch
            {
                IssueProvider.GitHub => _provider.GetService(typeof(GitHubIssueService)) as IIssueService
                    ?? throw new InvalidOperationException($"{nameof(GitHubIssueService)} is not registered."),
                IssueProvider.GitLab => _provider.GetService(typeof(GitLabIssueService)) as IIssueService
                    ?? throw new InvalidOperationException($"{nameof(GitLabIssueService)} is not registered."),
                _ => throw new NotSupportedException()
            };
        }
    }
}
