using System.Text;
using CommandLine;
using GithubAction;
using Microsoft.Extensions.Logging;
using Octokit;

public sealed class CustomAction
{
    private readonly ILogger<CustomAction> logger;
    private readonly ActionInputs actionInputs;

    public CustomAction(ILogger<CustomAction> logger, ActionInputs actionInputs)
    {
        this.logger = logger;
        this.actionInputs = actionInputs;
    }

    public async Task Run()
    {
        logger.LogInformation("Begin");
        logger.LogInformation($"Name: '{actionInputs.Name}'");

        var updatedMetrics = true;
        var title = "Updated 2 projects";
        var summary = "Calculated code metrics on two projects.";

        // Write GitHub Action workflow outputs.
        var gitHubOutputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT");

        logger.LogInformation($"GITHUB_OUTPUT: '{gitHubOutputFile}'");

        if (!string.IsNullOrWhiteSpace(gitHubOutputFile))
        {
            using StreamWriter textWriter = new(gitHubOutputFile, true, Encoding.UTF8);
            textWriter.WriteLine($"updated-metrics={updatedMetrics}");
            textWriter.WriteLine($"summary-title={title}");
            textWriter.WriteLine($"summary-details={summary}");
        }

        var client = new GitHubClient(new ProductHeaderValue("github-action"));
        var accessToken = Environment.GetEnvironmentVariable("ACCESS_TOKEN");
        client.Credentials = new Credentials(accessToken);
        var followerCount = (await client.User.Followers.GetAll("timmoth")).Count;
        var a = await client.User.Get("timmoth");
        
        var aa = await client.Activity.Events.GetAllUserPerformed("timmoth", new ApiOptions()
        {
            PageSize = 100,
            PageCount = 100
        });
        var output = new StringBuilder();
        foreach( var e in aa)
        {
            output.AppendLine($"{e.CreatedAt} - {e.Repo.Name} {e.Type}");
        }
        File.WriteAllText($"./activity.txt", output.ToString());

        logger.LogInformation("End");
    }
}