# Serilog.Enrichers.XRayLinkInCloudwatch

A Serilog enricher that appends the AWS X-Ray URL to your logs with the active TraceId.

This is especially useful when viewing structured logs in AWS CloudWatch, allowing you to navigate from a log directly to the AWS X-Ray console link.

## How it works

The enricher automatically extracts the current trace ID and creates a new log property (`XRayUrl`) with a direct link to the AWS X-Ray console in the following format:
```
https://console.aws.amazon.com/xray/home?region={region}#/traces/{traceId}
```

### Region Evaluation Order

The `{region}` is determined in the following order:
1. The explicit parameter passed to the `.WithXRayLinkInCloudwatch(awsRegion)` extension method.
2. The `AWS_REGION` environment variable.
3. The `AWS_DEFAULT_REGION` environment variable.
4. If none of the above are found, it defaults to `"us-east-1"`.

## Usage

Configure Serilog in your application setup and add `.Enrich.WithXRayLinkInCloudwatch()`:

```csharp
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .Enrich.WithXRayLinkInCloudwatch()
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});
```

To specify a different region, users can pass a parameter to the extension method:

```csharp
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .Enrich.WithXRayLinkInCloudwatch("eu-west-2")
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});
```