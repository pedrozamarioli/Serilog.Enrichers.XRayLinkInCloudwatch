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
1. The explicit parameter passed to the `.WithXRayLinkInCloudwatch(awsRegion)` enricher.
2. The `AWS_REGION` environment variable.
3. The `AWS_DEFAULT_REGION` environment variable.
4. If none of the above are found, it defaults to `"us-east-1"`.

## Usage

**1 ) In code**

Configure Serilog in your application setup and add `.Enrich.WithXRayLinkInCloudwatch()`

**1.1)** Without specifying a region (it will use the environment variables or default to "us-east-1"):

```csharp
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .Enrich.WithXRayLinkInCloudwatch()
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});
```

**1.2)** To specify a different region, pass a parameter (awsRegion) to the enricher (e.g.: "eu-west-2"):

```csharp
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .Enrich.WithXRayLinkInCloudwatch("eu-west-2")
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});
```

**2 )** In appsettings.json file:

**2.1)** Without specifying a region (it will use the environment variables or default to "us-east-1"):
```
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" }
    ],
    "Enrich": [ "WithXRayLinkInCloudwatch" ]
  }
```

**2.2)** To specify a different region, pass the awsRegion argument to the enricher (e.g.: "eu-west-2"):
```
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" }
    ],
    "Enrich": [      
      {
        "Name": "WithXRayLinkInCloudwatch",
        "Args": {
          "awsRegion": "eu-west-2"
        }
      }
    ] 
  }
```