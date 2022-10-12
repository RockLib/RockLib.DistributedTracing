using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using OpenTelemetry.Trace;
using System;
using System.Text.RegularExpressions;

namespace RockLib.DistributedTracing.AspNetCore
{
   using static HeaderNames;

   /// <summary>
   /// Extensions for <see cref="HttpContext"/>.
   /// </summary>
   public static class HttpContextExtensions
   {
      private static readonly Regex BLANK_TRACE_REGEX = new("^0+$");

      /// <summary>
      /// Gets the correlation id from an <see cref="HttpContext"/>.
      /// </summary>
      /// <param name="httpContext">The http context.</param>
      /// <returns>The correlation id.</returns>
      /// <param name="correlationIdHeader">The name of the correlation id header.</param>
      [System.Obsolete("Applications aligning to Dynatrace should use TraceId and SpanId instead of CorrelationId")]
      public static string? GetCorrelationId(this HttpContext? httpContext, string correlationIdHeader = CorrelationId) =>
         httpContext?.GetCorrelationIdAccessor(correlationIdHeader).CorrelationId;

      /// <summary>
      /// Sets the correlation id to an <see cref="HttpContext"/>.
      /// </summary>
      /// <param name="httpContext">The http context.</param>
      /// <param name="correlationId">The correlation id.</param>
      /// <param name="correlationIdHeader">The name of the correlation id header.</param>
      [System.Obsolete("Applications aligning to Dynatrace should use TraceId and SpanId instead of CorrelationId")]
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
         {
            throw new ArgumentNullException(nameof(httpContext));
         }
         if (correlationIdHeader is null)
         {
            throw new ArgumentNullException(nameof(correlationIdHeader));
         }

         if (!httpContext.Items.TryGetValue(typeof(ICorrelationIdAccessor), out var value) 
            || value is not ICorrelationIdAccessor accessor)
         {
            accessor = new CorrelationIdAccessor();

            // align w/OpenTelemetry
            accessor.TraceId = GetTraceId(httpContext);
            accessor.SpanId = GetSpanId(httpContext);

#pragma warning disable CS0618 // Type or member is obsolete
            if (httpContext.GetHeaderValue(correlationIdHeader) is StringValues correlationId && correlationId.Count > 0)
            {
               accessor.CorrelationId = correlationId;
            }

            if (accessor.CorrelationId is null)
            {
               accessor.CorrelationId = httpContext.GetTraceId(); // favor Otel alignment
            }

            if (accessor.CorrelationId is null)
            {
               accessor.CorrelationId = Guid.NewGuid().ToString();
            }
#pragma warning restore CS0618 // Type or member is obsolete

            httpContext.Items[typeof(ICorrelationIdAccessor)] = accessor;
         }

         return accessor;
      }

      /// <summary>
      /// Exposes OpenTelemtry's TraceId to downstream Rocklib consumers
      /// </summary>
      /// <param name="httpContext"></param>
      /// <returns></returns>
      public static string? GetTraceId(this HttpContext httpContext)
      {
         var traceId = Tracer.CurrentSpan?.Context.TraceId.ToString();
         return (traceId is not null && !BLANK_TRACE_REGEX.IsMatch(traceId)) ? traceId : null;
      }

      /// <summary>
      /// Exposes OpenTelemtry's SpanId to downstream Rocklib consumers
      /// </summary>
      /// <param name="httpContext"></param>
      /// <returns></returns>
      public static string? GetSpanId(this HttpContext httpContext)
      {
         var spanId = Tracer.CurrentSpan?.Context.SpanId.ToString();
         return (spanId is not null && !BLANK_TRACE_REGEX.IsMatch(spanId)) ? spanId : null;
      }

      private static StringValues GetHeaderValue(this HttpContext httpContext, string headerName) =>
          httpContext?.Request?.Headers is IHeaderDictionary headers
          && headers.TryGetValue(headerName, out var headerValue)
              ? headerValue
              : default;
   }
}
