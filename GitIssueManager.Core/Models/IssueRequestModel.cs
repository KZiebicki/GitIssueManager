namespace GitIssueManager.Core.Models
{
    public class IssueRequestModel
    {
        required public IssueProvider Provider { get; set; } = IssueProvider.GitHub;
        required public string Repository { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
