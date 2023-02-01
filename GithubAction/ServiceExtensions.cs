using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public static class ServiceExtensions
{
    public static TService Get<TService>(this IHost host)
    where TService : notnull =>
    host.Services.GetRequiredService<TService>();

    public static ILogger GetLogger(this IHost host) => host.Get<ILoggerFactory>().CreateLogger("Github Action");
}
