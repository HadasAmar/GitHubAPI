using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Octokit;
using Service.Configuration;
using Service.Entities;

namespace Service.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly IGitHubClient _gitHubClient;
        private readonly ApiCredentials _options;

        public GitHubService(IOptions<ApiCredentials> options)
        {
            _options = options.Value;
            _gitHubClient = new GitHubClient(new ProductHeaderValue("MyApp"))
            {

                Credentials = new Credentials(_options.GitHubToken)
            };
        }

        public async Task<int> GetUserFollowersAsync()
        {
            var user = await _gitHubClient.User.Get(_options.UserName);
            return user.Followers;
        }

        public async Task<List<RepoDetails>> GetUserRepositoriesAsync()
        {
            var repositories = await _gitHubClient.Repository.GetAllForUser(_options.UserName);
            var result = new List<RepoDetails>();

            foreach (var repo in repositories)
            {
                var lastCommit = await _gitHubClient.Repository.Commit.GetAll(repo.Id);
                var languages = await _gitHubClient.Repository.GetAllLanguages(repo.Id);
                var pullRequests = await _gitHubClient.PullRequest.GetAllForRepository(repo.Id);

                result.Add(new RepoDetails
                {
                    Name = repo.Name,
                    Url = repo.HtmlUrl,
                    Stars = repo.StargazersCount,
                    LastCommitDate = lastCommit.FirstOrDefault()?.Commit.Committer.Date.UtcDateTime,
                    Languages = languages.Select(lang => lang.Name).ToList(),
                    PullRequestCount = pullRequests.Count
                });
            }
            return result;
        }

        public async Task<List<RepoDetails>> SearchRepositories(string? repoName = null, string? language = null, string? userName = null)
        {
            var queryParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(repoName))
                queryParts.Add($"{repoName} in:name");

            if (!string.IsNullOrWhiteSpace(language))
                queryParts.Add($"language:{language}");

            if (!string.IsNullOrWhiteSpace(userName))
                queryParts.Add($"user:{userName}");

            var query = string.Join(" ", queryParts);

            if (string.IsNullOrWhiteSpace(query))
                query = "stars:>0"; //ברירת מחדל..

            var request = new SearchRepositoriesRequest(query);
            var repositories = await _gitHubClient.Search.SearchRepo(request);

            var result = new List<RepoDetails>();

            foreach (var repo in repositories.Items)
            {
                var lastCommit = await _gitHubClient.Repository.Commit.GetAll(repo.Id);
                var languages = await _gitHubClient.Repository.GetAllLanguages(repo.Id);
                var pullRequests = await _gitHubClient.PullRequest.GetAllForRepository(repo.Id);

                result.Add(new RepoDetails
                {
                    Name = repo.Name,
                    Url = repo.HtmlUrl,
                    Stars = repo.StargazersCount,
                    LastCommitDate = lastCommit.FirstOrDefault()?.Commit.Committer.Date.UtcDateTime,
                    Languages = languages.Select(lang => lang.Name).ToList(),
                    PullRequestCount = pullRequests.Count
                });
            }
            return result;
        }


    }
}
