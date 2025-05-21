using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitIssueManager.Core.Services.Interfaces
{
    public interface IIssueServiceFactory
    {
        IIssueService GetService(IssueProvider provider);
    }

    public enum IssueProvider
    {
        GitHub,
        GitLab
    }
}
