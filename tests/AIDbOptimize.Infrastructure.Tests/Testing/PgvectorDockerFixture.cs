using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Npgsql;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests.Testing;

public sealed class PgvectorDockerFixture : IAsyncLifetime
{
    private string? _containerName;

    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitializeAsync()
    {
        var port = AllocatePort();
        _containerName = $"aidbopt-pgvector-test-{Guid.NewGuid():N}";
        var password = "postgres";
        ConnectionString = $"Host=127.0.0.1;Port={port};Username=postgres;Password={password};Database=postgres";

        await RunDockerAsync(
            $"run --name {_containerName} -e POSTGRES_PASSWORD={password} -e POSTGRES_USER=postgres -p {port}:5432 -d pgvector/pgvector:pg17");
        await WaitUntilReadyAsync();
    }

    public async Task DisposeAsync()
    {
        if (string.IsNullOrWhiteSpace(_containerName))
        {
            return;
        }

        await RunDockerAsync($"rm -f {_containerName}", ignoreErrors: true);
    }

    public async Task<string> CreateDatabaseAsync()
    {
        var databaseName = $"aidbopt_rag_{Guid.NewGuid():N}";
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using (var command = connection.CreateCommand())
        {
            command.CommandText = $"CREATE DATABASE \"{databaseName}\";";
            await command.ExecuteNonQueryAsync();
        }

        return $"{ConnectionString};Database={databaseName}";
    }

    private static int AllocatePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }

    private async Task WaitUntilReadyAsync()
    {
        var attempts = 0;
        while (attempts++ < 30)
        {
            try
            {
                await using var connection = new NpgsqlConnection(ConnectionString);
                await connection.OpenAsync();
                await using var command = connection.CreateCommand();
                command.CommandText = "SELECT 1;";
                await command.ExecuteScalarAsync();
                return;
            }
            catch
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }

        throw new InvalidOperationException("pgvector docker container did not become ready in time.");
    }

    private static async Task RunDockerAsync(string arguments, bool ignoreErrors = false)
    {
        var startInfo = new ProcessStartInfo("docker", arguments)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Failed to start docker process.");
        var stdout = await process.StandardOutput.ReadToEndAsync();
        var stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0 && !ignoreErrors)
        {
            throw new InvalidOperationException($"docker {arguments} failed.{Environment.NewLine}{stdout}{Environment.NewLine}{stderr}");
        }
    }
}
