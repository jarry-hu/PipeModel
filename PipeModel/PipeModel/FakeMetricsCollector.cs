using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipeModel
{
    public class FakeMetricsCollector :
        IProcessorMetricsCollector,
        IMemoryMetricsCollector,
        INetworkMetricsCollector
    {
        int IProcessorMetricsCollector.GetUsage()
        => PerformanceMetrics.Create().Processor;

        long INetworkMetricsCollector.GetThroughput()
        => PerformanceMetrics.Create().Network;

        long IMemoryMetricsCollector.GetUsage()
        => PerformanceMetrics.Create().Memory;

    }

    public class FakeMetricsDeliverer : IMetricsDeliverer
    {
        private readonly TransportType _transport;
        private readonly EndPoint _deliverTo;
        private readonly ILogger _logger;
        private readonly Action<ILogger, DateTimeOffset, PerformanceMetrics, EndPoint,
            TransportType, Exception> _logForDelivery;
        public FakeMetricsDeliverer(
            IOptions<MetricsCollectionOptions> optionsAccessor,
            ILogger<FakeMetricsDeliverer> logger)
        {
            var options = optionsAccessor.Value;
            _transport = options.Transport;
            _deliverTo = options.DeliverTo;
            _logger = logger;
            _logForDelivery = LoggerMessage.Define<DateTimeOffset, PerformanceMetrics,
                EndPoint, TransportType>(LogLevel.Information, 0,
                "[{0}]Deliver performance counter {1} to {2} via {3}");
        }
        public Task DeliverAsync(PerformanceMetrics counter)
        {
            _logForDelivery(_logger, DateTimeOffset.Now, counter, _deliverTo, _transport, null);
            //Console.WriteLine($"[{DateTimeOffset.UtcNow}]Deliver performance counter {counter}" +
            //    $"to {_deliverTo} via {_transport}");
            return Task.CompletedTask;
        }
    }
}
