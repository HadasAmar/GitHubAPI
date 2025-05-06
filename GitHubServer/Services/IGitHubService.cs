using Octokit;
using Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IGitHubService
    {
        Task<int> GetUserFollowersAsync();
        Task<List<RepoDetails>> GetUserRepositoriesAsync();
        Task<List<RepoDetails>> SearchRepositories(string repoName, string language,string userName);
    }
}
