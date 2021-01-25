namespace RockLib.DistributedTracing.AspNetCore.Config
{
    public class TracerConfig
    {
        public string Exporter { get; set; }

        public string Environment { get; set; }

        public string ApplicationId { get; set; }

        public string ServiceName { get; set; }

        public string ServiceEndpoint { get; set; }
    }
}
