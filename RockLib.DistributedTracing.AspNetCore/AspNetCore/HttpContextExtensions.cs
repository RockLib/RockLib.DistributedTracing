using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;

namespace RockLib.DistributedTracing.AspNetCore
{
   using static HeaderNames;

   /// <summary>
   /// Extensions for <see cref="HttpContext"/>.
   /// </summary>
   public static class HttpContextExtensions
   {
      /// <summary>
      /// Gets the correlation id from an <see cref="HttpContext"/>.
      /// </summary>
      /// <param name="httpContext">The http context.</param>
      /// <returns>The correlation id.</returns>
      /// <param name="correlationIdHeader">The name of the correlation id header.</param>
      public static string? GetCorrelationId(this HttpContext? httpContext, string correlationIdHeader = CorrelationId) =>
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

            if (httpContext.GetHeaderValue(correlationIdHeader) is StringValues correlationId && correlationId.Count > 0)
            {
               accessor.CorrelationId = correlationId;
            }

            if (accessor.CorrelationId is null)
            {
               accessor.CorrelationId = Guid.NewGuid().ToString();
            }

            httpContext.Items[typeof(ICorrelationIdAccessor)] = accessor;
         }

         return accessor;
      }

      private static StringValues GetHeaderValue(this HttpContext httpContext, string headerName) =>
          httpContext?.Request?.Headers is IHeaderDictionary headers
          && headers.TryGetValue(headerName, out var headerValue)
              ? headerValue
              : default;
   }
}
