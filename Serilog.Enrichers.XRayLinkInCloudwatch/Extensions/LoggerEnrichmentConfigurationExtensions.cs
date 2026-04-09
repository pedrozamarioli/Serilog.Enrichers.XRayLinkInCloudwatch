using Serilog.Configuration;

namespace Serilog.Enrichers.XRayLinkInCloudwatch;

public static class LoggerEnrichmentConfigurationExtensions
{
    public static LoggerConfiguration WithXRayLinkInCloudwatch(
        this LoggerEnrichmentConfiguration enrichmentConfiguration,
        string? awsRegion = null)
    {
        return enrichmentConfiguration.With(new XRayLinkInCloudwatchEnricher(awsRegion));
    }
}

