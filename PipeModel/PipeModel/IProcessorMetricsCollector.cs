using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipeModel
{
    public interface IProcessorMetricsCollector
    {
        int GetUsage();
    }
    public interface IMemoryMetricsCollector
    {
        long GetUsage();
    }
    public interface INetworkMetricsCollector
    {
        long GetThroughput();
    }
    public interface IMetricsDeliverer
    {
        Task DeliverAsync(PerformanceMetrics counter);
    }
}
