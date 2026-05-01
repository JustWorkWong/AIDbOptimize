using AIDbOptimize.ApiService.Configuration;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AIDbOptimize.ApiService.Tests;

public sealed class ControlPlaneConnectionStringResolverTests
{
    [Fact]
    public void Resolve_PrefersConfiguredLocalOverride_OverFallbackDefault()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ControlPlane"] = "Host=override;Database=localdb",
                ["ConnectionStrings:aidbopt-control"] = "Host=primary;Database=primarydb"
            })
            .Build();

        var result = ControlPlaneConnectionStringResolver.Resolve(
            configuration,
            "aidbopt-control",
            "ControlPlane",
            "Host=default;Database=defaultdb");

        Assert.Equal("Host=primary;Database=primarydb", result);
    }

    [Fact]
    public void Resolve_FallsBackToNamedOverride_WhenPrimaryMissing()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:ControlPlane"] = "Host=override;Database=localdb"
            })
            .Build();

        var result = ControlPlaneConnectionStringResolver.Resolve(
            configuration,
            "aidbopt-control",
            "ControlPlane",
            "Host=default;Database=defaultdb");

        Assert.Equal("Host=override;Database=localdb", result);
    }
}
