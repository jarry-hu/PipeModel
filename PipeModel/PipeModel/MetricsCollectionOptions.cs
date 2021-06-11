using System;
namespace PipeModel
{
    public class MetricsCollectionOptions
    {
        public TimeSpan CaptureInterval { get; set; }
        public TransportType Transport { get; set; }
        public EndPoint DeliverTo { get; set; }
    }
    public enum TransportType
    {
        Tcp,
        Http,
        Udp
    }

    public class EndPoint
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public override string ToString()=>$"{Host}:{Port}";
    }
}
