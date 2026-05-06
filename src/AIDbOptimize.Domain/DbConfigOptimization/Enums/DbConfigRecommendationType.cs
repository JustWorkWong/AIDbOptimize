using System.Text.Json.Serialization;

namespace AIDbOptimize.Domain.DbConfigOptimization.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<DbConfigRecommendationType>))]
public enum DbConfigRecommendationType
{
    ActionableRecommendation = 0,
    RequestMoreContext = 1
}
