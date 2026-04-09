using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace Serilog.Enrichers.XRayLinkInCloudwatch;

public class XRayLinkInCloudwatchEnricher : ILogEventEnricher
{
    private readonly string _region;

    public XRayLinkInCloudwatchEnricher(string? awsRegion = null)
    {
        // Detect AWS region from parameter, then environment variables
        _region = awsRegion
               ?? Environment.GetEnvironmentVariable("AWS_REGION")
               ?? Environment.GetEnvironmentVariable("AWS_DEFAULT_REGION")
               ?? "us-east-1"; // fallback
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;
        if (activity != null && !string.IsNullOrWhiteSpace(activity.TraceId.ToString()))
        {
            var traceId = activity.TraceId.ToString();
            var xrayUrl = $"https://console.aws.amazon.com/xray/home?region={_region}#/traces/{traceId}";

            var prop = propertyFactory.CreateProperty("XRayUrl", xrayUrl);
            logEvent.AddPropertyIfAbsent(prop);
        }
    }
}
