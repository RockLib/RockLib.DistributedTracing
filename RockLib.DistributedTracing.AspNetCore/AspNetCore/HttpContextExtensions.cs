using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System;
using System.Net;
using static Microsoft.Net.Http.Headers.HeaderNames;

namespace RockLib.DistributedTracing.AspNetCore
{
    using static HeaderNames;

    /// <summary>
    /// Extensions for <see cref="HttpContext"/>.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the http request method from an <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The http request method.</returns>
        public static string GetMethod(this HttpContext httpContext) =>
            httpContext?.Request?.Method;

        /// <summary>
        /// Gets the http request path from an <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The http request path.</returns>
        public static string GetPath(this HttpContext httpContext)
        {
            if (httpContext?.Features[typeof(IEndpointFeature)] is IEndpointFeature endpointFeature
                && endpointFeature.Endpoint is RouteEndpoint endpoint)
                return endpoint.RoutePattern.RawText;

            return httpContext.Request?.Path;
        }

        /// <summary>
        /// Gets the http user agent header from an <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The http user agent header.</returns>
        public static StringValues GetUserAgent(this HttpContext httpContext) =>
            httpContext.GetHeaderValue(UserAgent);

        /// <summary>
        /// Gets the http referrer header from an <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The http referrer header.</returns>
        public static Uri GetReferrer(this HttpContext httpContext) =>
            httpContext?.Request?.GetTypedHeaders() is RequestHeaders headers
                ? headers.Referer
                : null;

        /// <summary>
        /// Gets the correlation id from an <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The correlation id.</returns>
        /// <param name="correlationIdHeader">The name of the correlation id header.</param>
        public static string GetCorrelationId(this HttpContext httpContext, string correlationIdHeader = CorrelationId) =>
            httpContext?.GetCorrelationIdAccessor(correlationIdHeader).CorrelationId;

        /// <summary>
        /// Sets the correlation id to an <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="correlationIdHeader">The name of the correlation id header.</param>
        public static void SetCorrelationId(this HttpContext httpContext, string correlationId, string correlationIdHeader = CorrelationId) =>
            httpContext.GetCorrelationIdAccessor(correlationIdHeader).CorrelationId = correlationId;

        /// <summary>
        /// Gets the accessor used to retreive a correlation id from an <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The correlation id accessor.</returns>
        /// <param name="correlationIdHeader">The name of the correlation id header.</param>
        public static ICorrelationIdAccessor GetCorrelationIdAccessor(this HttpContext httpContext, string correlationIdHeader = CorrelationId)
        {
            if (httpContext is null)
                throw new ArgumentNullException(nameof(httpContext));
            if (correlationIdHeader is null)
                throw new ArgumentNullException(nameof(correlationIdHeader));

            if (!httpContext.Items.TryGetValue(typeof(ICorrelationIdAccessor), out var value)
                || !(value is ICorrelationIdAccessor accessor))
            {
                accessor = new CorrelationIdAccessor();

                if (httpContext.GetHeaderValue(correlationIdHeader) is StringValues correlationId && correlationId.Count > 0)
                    accessor.CorrelationId = correlationId;

                if (accessor.CorrelationId is null)
                    accessor.CorrelationId = Guid.NewGuid().ToString();

                httpContext.Items[typeof(ICorrelationIdAccessor)] = accessor;
            }

            return accessor;
        }

        /// <summary>
        /// Gets the connection remote ip address from an <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The remote ip address.</returns>
        public static IPAddress GetRemoteIpAddress(this HttpContext httpContext) =>
            httpContext?.Connection?.RemoteIpAddress;

        /// <summary>
        /// Gets the forwarded for header from an <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The forwarded for header.</returns>
        public static StringValues GetForwardedFor(this HttpContext httpContext) =>
            httpContext.GetHeaderValue(ForwardedFor);

        private static StringValues GetHeaderValue(this HttpContext httpContext, string headerName) =>
            httpContext?.Request?.Headers is IHeaderDictionary headers
            && headers.TryGetValue(headerName, out var headerValue)
                ? headerValue
                : default(StringValues);
    }
}
