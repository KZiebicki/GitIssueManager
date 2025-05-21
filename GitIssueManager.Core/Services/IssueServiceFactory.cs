using GitIssueManager.Core.Services.Interfaces;

namespace GitIssueManager.Core.Services
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
                IssueProvider.GitHub => _provider.GetService(typeof(GitHubIssueService)) as IIssueService,
                IssueProvider.GitLab => _provider.GetService(typeof(GitLabIssueService)) as IIssueService,
                _ => throw new NotSupportedException()
            };
        }
    }
}
