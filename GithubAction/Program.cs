using System.Text;
using CommandLine;
using GithubAction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static CommandLine.Parser;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => { services.AddLogging(); })
    .Build();

var parser = Default.ParseArguments<ActionInputs>(() => new(), args);
parser.WithNotParsed(
    errors =>
    {
        host.GetLogger().LogError(
                string.Join(
                    Environment.NewLine, errors.Select(error => error.ToString())));

        Environment.Exit(2);
    });

await parser.WithParsedAsync(async options => await StartAnalysisAsync(options, host));
await host.RunAsync();

static async Task StartAnalysisAsync(ActionInputs inputs, IHost host)
{
    var logger = host.GetLogger();

    logger.LogInformation("Begin");

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

    logger.LogInformation("End");
    await Task.Delay(TimeSpan.FromMilliseconds(1));
    Environment.Exit(0);
}