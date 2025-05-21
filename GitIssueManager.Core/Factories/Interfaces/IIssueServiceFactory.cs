using GitIssueManager.Core.Models;
using GitIssueManager.Core.Services.Interfaces;

namespace GitIssueManager.Core.Factories.Interfaces
{
    public interface IIssueServiceFactory
    {
        IIssueService GetService(IssueProvider provider);
    }
}
