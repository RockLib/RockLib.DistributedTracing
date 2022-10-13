using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace RockLib.DistributedTracing.AspNetCore
{
   /// <summary>
   /// A middleware used to set the correlation id in the response header. The id will either be found
   /// in the request header, or if not found, generated.
   /// </summary>
   public class CorrelationIdMiddleware
   {
      private readonly RequestDelegate _next;

      /// <summary>
      /// Initializes a new instance of the <see cref="CorrelationIdMiddleware"/> class.
      /// </summary>
      /// <param name="next">The <see cref="RequestDelegate"/> used to process the HTTP request.</param>
      /// <param name="options">The options for the <see cref="CorrelationIdMiddleware"/> instance.</param>
      public CorrelationIdMiddleware(RequestDelegate next, IOptionsMonitor<CorrelationIdMiddlewareOptions>? options = null)
      {
         _next = next ?? throw new ArgumentNullException(nameof(next));

         var middlewareOptions = options?.CurrentValue ?? new CorrelationIdMiddlewareOptions();

         HeaderName = middlewareOptions.HeaderName ?? HeaderNames.CorrelationId;
      }

      /// <summary>
      /// Gets the name for the correlation id header.
      /// </summary>
      public string HeaderName { get; }

      /// <summary>
      /// Sets the correlation id header to the response, if missing.
      /// </summary>
      /// <param name="context">The HttpContext.</param>
#pragma warning disable CS0618 // only the ref to GetCorrelationId is obsolete here...
      public async Task InvokeAsync(HttpContext context)
      {
         var correlationId = context.GetCorrelationId(HeaderName);

         context.Response.OnStarting(() =>
         {
            if (string.IsNullOrWhiteSpace(context.Response.Headers[HeaderName]))
               context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
         });

         await _next(context).ConfigureAwait(false);
      }
#pragma warning restore CS0618 // Type or member is obsolete
   }
}