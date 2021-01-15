# How to access the correlation id of an HttpContext

### GetCorrelationIdAccessor

Accessing the correlation id of an `HttpContext` is done with the `ICorrelationIdAccessor` returned by the `GetCorrelationIdAccessor` extension method, from the `RockLib.DistributedTracing.AspNetCore` namespace.

```c#
ICorrelationIdAccessor accessor = HttpContext.GetCorrelationIdAccessor();
string correlationId = accessor.CorrelationId;
```

Each time the `GetCorrelationIdAccessor` extension method is called on the same instance of `HttpContext`, the same instance of `ICorrelationIdAccessor` is returned. This is because the accessor is stored in the `HttpContext.Items` dictionary.

The first time the `GetCorrelationIdAccessor` extension method is called, an attempt is made to set the value of its `CorrelationId` property from the `"X-Correlation-Id"` header from the `HttpContext.Request.Headers`. If no such header exists, then the value is set to a new GUID value.

A different header (besides `"X-Correlation-Id"`) can be specified by passing a value to the optional `correlationIdHeader` parameter:

```c#
ICorrelationIdAccessor accessor = HttpContext.GetCorrelationIdAccessor("MyCorrelationIdHeader");
```

### GetCorrelationId

This extension method, which has the same optional `correlationIdHeader` parameter as the above `GetCorrelationIdAccessor` extension method, returns the value of the `CorrelationId` property of the `ICorrelationIdAccessor` returned by a call to `GetCorrelationIdAccessor`.

```c#
string correlationId = HttpContext.GetCorrelationId();
```

### SetCorrelationId

This extension method is exactly like `GetCorrelationId`, except it sets the `CorrelationId` property of the `ICorrelationIdAccessor` instead of getting it.

```c#
HttpContext.SetCorrelationId("MyCorrelationId");
```
