
using System.Diagnostics.Metrics;

namespace ServicePlatform.Api.Monitoring;

public sealed class HealthStatusMetrics : IDisposable
{
    private readonly Meter _meter =
        new("EnterpriseServicePlatform.Health");

    private readonly ObservableGauge<int> _healthGauge;

    private int _healthStatus = 0;

    public HealthStatusMetrics()
    {
        _healthGauge = _meter.CreateObservableGauge(
            "service_platform_health_status",
            () => Volatile.Read(ref _healthStatus),
            description: "API health status: 1 = healthy, 0 = unhealthy");
    }

    public void SetHealthStatus(bool isHealthy)
    {
        Volatile.Write(ref _healthStatus, isHealthy ? 1 : 0);
    }

    public void Dispose()
    {
        _meter.Dispose();
    }
}
