namespace CITADEL_NEXUS_PUBLIC.Models;

public sealed class PublicOracleOptions
{
    public const string SectionName = "PublicOracle";

    public string Endpoint { get; set; } = string.Empty;

    public string PublicApiKey { get; set; } = string.Empty;
}
