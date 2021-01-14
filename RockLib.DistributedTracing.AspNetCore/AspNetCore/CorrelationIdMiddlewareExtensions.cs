using Microsoft.AspNetCore.Builder;

namespace RockLib.DistributedTracing.AspNetCore
{
    /// <summary>
    /// Provides extension methods for the <see cref="CorrelationIdMiddleware"/> class.
    /// </summary>
    public static class CorrelationIdMiddlewareExtensions
    {
        /// <summary>
        /// Adds <see cref="CorrelationIdMiddleware"/> to the application's request pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder builder) =>
            builder.UseMiddleware<CorrelationIdMiddleware>();
    }
}