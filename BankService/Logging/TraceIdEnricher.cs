using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace BankService.Logging;

/// <summary>
/// Enriches log events with the current <see cref="Activity"/> trace and span identifiers
/// so Serilog output can be correlated with OpenTelemetry traces.
/// </summary>
public sealed class TraceIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;
        if (activity is null)
        {
            return;
        }

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", activity.TraceId.ToString()));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("SpanId", activity.SpanId.ToString()));
    }
}
