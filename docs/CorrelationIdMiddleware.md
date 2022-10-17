# How to automatically propagate a correlation id using http headers

To automatically read a correlation id from an http request and then add it to the http response, use the `CorrelationIdMiddleware`.

Adding the `CorrelationsIdMiddleware` via application builder extension:

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseCorrelationIdMiddleware();
}
```

The middleware reads header `X-Correlation-Id` from the http request. If no correlation id is found in the request, or via Open Telemetry (as a fallback, aiming for alignment across applications), a new one will be created.  
The middleware will add this as the same header to the http response. The name of the header can be customized by using the CorrelationIdMiddlewareOptions.

If the correlationId is derived from Open Telemetry the format will be a 32-hex-character lowercase string.  
If the correlationId is created by the library it will be a GUID.

Customizing the header name:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<CorrelationIdMiddlewareOptions>(options => options.HeaderName = "Custom-Id-Header");
}
```

This would instead read and propagate the header `Custom-Id-Header`.