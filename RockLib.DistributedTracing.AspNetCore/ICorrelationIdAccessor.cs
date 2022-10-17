namespace RockLib.DistributedTracing
{
    /// <summary>
    /// Defines an object that used to retreive the correlation id.
    /// </summary>
    public interface ICorrelationIdAccessor
    {
        /// <summary>
        /// Gets the correlation id.
        /// </summary>
        string? CorrelationId { get; set; }

        /// <summary>
        /// Gets the Otel trace id.
        /// </summary>
        string? TraceId { get; set; }

        /// <summary>
        /// Gets the Otel span id.
        /// </summary>
        string? SpanId { get; set; }
    }
}
