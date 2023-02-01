using System.Text;
using GithubAction;
using Microsoft.Extensions.Logging;

public sealed class CustomAction
{
    private readonly ILogger<CustomAction> logger;
    private readonly ActionInputs actionInputs;

    public CustomAction(ILogger<CustomAction> logger, ActionInputs actionInputs)
    {
        this.logger = logger;
        this.actionInputs = actionInputs;
    }

    public Task Run()
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

        File.WriteAllText($"./{DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")}.txt", "hello, world!");

        logger.LogInformation("End");
        return Task.CompletedTask;
    }
}