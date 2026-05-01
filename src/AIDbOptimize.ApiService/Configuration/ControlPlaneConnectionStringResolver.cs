using Microsoft.Extensions.Configuration;

namespace AIDbOptimize.ApiService.Configuration;

/// <summary>
/// Resolves the control-plane connection string from configuration sources.
/// </summary>
public static class ControlPlaneConnectionStringResolver
{
    public static string Resolve(
        IConfiguration configuration,
        string primaryName,
        string fallbackName,
        string defaultValue)
    {
        return configuration.GetConnectionString(primaryName)
            ?? configuration.GetConnectionString(fallbackName)
            ?? defaultValue;
    }
}
