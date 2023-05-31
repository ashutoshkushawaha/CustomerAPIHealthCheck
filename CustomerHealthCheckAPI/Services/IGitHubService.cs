namespace CustomerHealthCheckAPI.Services
{
    public interface IGitHubService
    {
        Task<bool> IsValidGitHubUser(string username);
    }
}
