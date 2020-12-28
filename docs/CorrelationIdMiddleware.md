# How to automatically propagate a correlation id using http headers

To automatically read a correlation id from an http request and then add it to the http response, use the `CorrelationIdMiddleware`. If no correlation id is found in the request, a new one will be created instead.

Adding the `CorrelationsIdMiddleware` via application builder extension:

```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseCorrelationIdMiddleware();
}
```

By default, this will read the correlation id from the http request as the header `X-Correlation-Id`. Then, it will add this as the same header to the http response. The name of the header can be customized by using the CorrelationIdMiddlewareOptions.

Customizing the header name:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.Configure<CorrelationIdMiddlewareOptions>(options => options.HeaderName = "Custom-Id-Header");
}
```

This would instead read and propagate the header `Custom-Id-Header`.