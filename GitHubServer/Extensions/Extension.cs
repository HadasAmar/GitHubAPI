using Microsoft.Extensions.DependencyInjection;
using Service.Configuration;
using Service.Services;
using System;

namespace Service.Extensions
{
    public static class Extension
    {
        public static void AddApiCredentials(this IServiceCollection services, Action<ApiCredentials> configureOption)
        {
            services.Configure(configureOption);
            services.AddScoped<IGitHubService, GitHubService>();
        }
    }
}
