using GithubAction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static CommandLine.Parser;

var parser = Default.ParseArguments<ActionInputs>(() => new(), args);
if (parser.Errors.Any())
{
    Console.Error.WriteLine(string.Join(
            Environment.NewLine, parser.Errors.Select(error => error.ToString())));
    Environment.Exit(2);
}

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => { 
        services.AddLogging();
        services.AddScoped<CustomAction>();
        services.AddSingleton(parser.Value);
    })
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
IServiceProvider provider = serviceScope.ServiceProvider;
var customAction = provider.GetRequiredService<CustomAction>();
await customAction.Run();
Environment.Exit(0);