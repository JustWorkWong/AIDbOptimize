using System.Globalization;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// Normalized read helper for db-config rules.
/// </summary>
public sealed class DbConfigCollectedContext
{
    private readonly Dictionary<string, DbConfigEvidenceItem> _evidenceByReference;
    private readonly IReadOnlyDictionary<string, string> _values;

    public DbConfigCollectedContext(DbConfigSnapshot snapshot)
    {
        Snapshot = snapshot;
        Engine = snapshot.Engine;
        DatabaseName = snapshot.DatabaseName;
        _values = snapshot.CollectedValues;
        _evidenceByReference = snapshot.ConfigurationItems
            .Concat(snapshot.RuntimeMetricItems)
            .Concat(snapshot.HostContextItems)
            .Concat(snapshot.ObservabilityItems)
            .GroupBy(item => item.Reference, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Last(), StringComparer.OrdinalIgnoreCase);
    }

    public DbConfigSnapshot Snapshot { get; }

    public DatabaseEngine Engine { get; }

    public string DatabaseName { get; }

    public bool HasHostContext => Snapshot.HostContextItems.Count > 0;

    public bool HasObservability => Snapshot.ObservabilityItems.Count > 0;

    public IReadOnlyList<DbConfigMissingContextItem> MissingContextItems => Snapshot.MissingContextItems;

    public string? GetValue(string key)
    {
        if (_evidenceByReference.TryGetValue(key, out var item))
        {
            return item.NormalizedValue ?? item.RawValue;
        }

        return _values.TryGetValue(key, out var value) ? value : null;
    }

    public bool TryGetInt64(string key, out long value)
    {
        var raw = GetValue(key);
        if (long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
        {
            return true;
        }

        value = 0;
        return false;
    }

    public bool TryGetDecimal(string key, out decimal value)
    {
        var raw = GetValue(key);
        if (decimal.TryParse(raw, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
        {
            return true;
        }

        value = 0;
        return false;
    }

    public bool TryGetBytes(string key, out long value)
    {
        var raw = GetValue(key);
        if (string.IsNullOrWhiteSpace(raw))
        {
            value = 0;
            return false;
        }

        raw = raw.Trim();
        if (long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
        {
            return true;
        }

        var match = System.Text.RegularExpressions.Regex.Match(raw, @"^(?<value>[\d.]+)\s*(?<unit>[KMGTP]?B?)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            value = 0;
            return false;
        }

        var numeric = decimal.Parse(match.Groups["value"].Value, CultureInfo.InvariantCulture);
        var unit = match.Groups["unit"].Value.ToUpperInvariant();
        var scale = unit switch
        {
            "" or "B" => 1m,
            "KB" or "K" => 1024m,
            "MB" or "M" => 1024m * 1024m,
            "GB" or "G" => 1024m * 1024m * 1024m,
            "TB" or "T" => 1024m * 1024m * 1024m * 1024m,
            _ => 1m
        };
        value = (long)(numeric * scale);
        return true;
    }

    public bool TryGetBoolean(string key, out bool value)
    {
        var raw = GetValue(key);
        if (string.IsNullOrWhiteSpace(raw))
        {
            value = false;
            return false;
        }

        raw = raw.Trim();
        if (bool.TryParse(raw, out value))
        {
            return true;
        }

        if (string.Equals(raw, "ON", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(raw, "1", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(raw, "enabled", StringComparison.OrdinalIgnoreCase))
        {
            value = true;
            return true;
        }

        if (string.Equals(raw, "OFF", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(raw, "0", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(raw, "disabled", StringComparison.OrdinalIgnoreCase))
        {
            value = false;
            return true;
        }

        value = false;
        return false;
    }

    public decimal? TryGetRatio(string numeratorKey, string denominatorKey)
    {
        if (!TryGetDecimal(numeratorKey, out var numerator) || !TryGetDecimal(denominatorKey, out var denominator) || denominator <= 0)
        {
            return null;
        }

        return numerator / denominator;
    }

    public IReadOnlyList<string> BuildEvidenceReferences(params string[] references)
    {
        return references
            .Where(reference => _evidenceByReference.ContainsKey(reference))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    public bool HasEvidence(string reference)
    {
        return _evidenceByReference.ContainsKey(reference);
    }
}
