namespace RockLib.DistributedTracing.AspNetCore
{
    /// <summary>
    /// Defines the options for the <see cref="CorrelationIdMiddleware"/> class.
    /// </summary>
    public class CorrelationIdMiddlewareOptions
    {
        /// <summary>
        /// Gets or sets the name for the correlation id header.
        /// </summary>
        public string HeaderName { get; set; }
    }
}