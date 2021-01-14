namespace RockLib.DistributedTracing.AspNetCore
{
    /// <summary>
    /// A class for http header names.
    /// </summary>
    public static class HeaderNames
    {
        /// <summary>
        /// The forwarded for http header name.
        /// </summary>
        public const string ForwardedFor = "X-Forwarded-For";

        /// <summary>
        /// The correlation id http header name.
        /// </summary>
        public const string CorrelationId = "X-Correlation-Id";
    }
}
