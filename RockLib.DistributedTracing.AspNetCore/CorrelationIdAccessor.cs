namespace RockLib.DistributedTracing
{
    /// <summary>
    /// The default implementation of <see cref="ICorrelationIdAccessor"/> with a simple get and set property.
    /// </summary>
    public class CorrelationIdAccessor : ICorrelationIdAccessor
    {
        /// <summary>
        /// Gets the correlation id.
        /// </summary>
        public string? CorrelationId { get; set; }

        /// <summary>
        /// Gets the Otel trace id.
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// Gets the Otel span id.
        /// </summary>
        public string? SpanId { get; set; }
    }
}
