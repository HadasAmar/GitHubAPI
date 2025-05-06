using Microsoft.AspNetCore.Mvc;
using Service.Services;
namespace GitHubApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GitHubController : ControllerBase
    {


        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet("getFollowers")]

        public async Task<IActionResult> GetFollowers()
        {
            var followers = await _gitHubService.GetUserFollowersAsync();
            return Ok(followers);
        }

        [HttpGet("getRepositories")]
        public async Task<IActionResult> GetRepositories()
        {
            var repositories = await _gitHubService.GetUserRepositoriesAsync();
            return Ok(repositories);
        }

        [HttpGet("searchRepositories")]
        public async Task<IActionResult> SearchRepositories(string? repoName=null, string? language=null, string? userName=null)
        {
            var repositories = await _gitHubService.SearchRepositories(repoName, language, userName);
            return Ok(repositories);
        }

    }
}
