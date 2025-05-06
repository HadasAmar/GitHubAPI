using Microsoft.Extensions.Caching.Memory;
using Octokit;
using Service.Entities;
using Service.Services;

namespace GitHubApi.CatchedServices
{
    public class CachedGitHubServices : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;

        public CachedGitHubServices(IGitHubService gitHubService, IMemoryCache memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }
        public async Task<int> GetUserFollowersAsync()
        {
            var cacheKey = "followers";

            if (!_memoryCache.TryGetValue(cacheKey, out int followers)){

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromSeconds(30))
                   .SetSlidingExpiration(TimeSpan.FromSeconds(30));

                followers = await _gitHubService.GetUserFollowersAsync();

                _memoryCache.Set(cacheKey, followers, cacheEntryOptions);
            }
           
            return followers;
        }


        public Task<List<RepoDetails>> GetUserRepositoriesAsync()
        {
            return _gitHubService.GetUserRepositoriesAsync();
        }

        public Task<List<RepoDetails>> SearchRepositories(string repoName, string language, string userName)
        {
            return _gitHubService.SearchRepositories(repoName, language, userName);
        }
    }
}
