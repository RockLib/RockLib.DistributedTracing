using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace RockLib.DistributedTracing.AspNetCore.Tests
{
    public class HttpContextExtensionTests
    {
        [Fact(DisplayName = "GetMethod returns HttpContext.Request.Method")]
        public void GetMethod()
        {
            var requestMethod = "SomeRequestMethodValue1";

            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(rm => rm.Method).Returns(requestMethod);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Request).Returns(requestMock.Object);

            contextMock.Object.GetMethod().Should().Be(requestMethod);
        }

        [Fact(DisplayName = "GetPath returns HttpContext.Features.RoutePattern.RawText if possible")]
        public void GetPath1()
        {
            var path = new StringValues("SomePathValue1");

            var endpointMock = new Mock<IEndpointFeature>();
            endpointMock.Setup(em => em.Endpoint).Returns(new RouteEndpoint(rd => Task.CompletedTask, RoutePatternFactory.Pattern(path), 0, null, null) { });

            var featureMock = new Mock<IFeatureCollection>();
            featureMock.Setup(fm => fm[typeof(IEndpointFeature)]).Returns(endpointMock.Object);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Features).Returns(featureMock.Object);

            contextMock.Object.GetPath().Should().Be(path);
        }

        [Fact(DisplayName = "GetPath returns HttpContext.Request.Path")]
        public void GetPath2()
        {
            var path = "/SomePathValue1";

            var featureMock = new Mock<IFeatureCollection>();

            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(rm => rm.Path).Returns(path);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Features).Returns(new Mock<IFeatureCollection>().Object);
            contextMock.Setup(cm => cm.Request).Returns(requestMock.Object);

            contextMock.Object.GetPath().Should().Be(path);
        }

        [Fact(DisplayName = "GetHeaderAgent returns user agent header value")]
        public void GetHeaderAgent()
        {
            var userAgent = new StringValues("SomeUserAgentValue1");

            var headers = new RequestHeaders(new HeaderDictionary());
            headers.Set(Microsoft.Net.Http.Headers.HeaderNames.UserAgent, userAgent);

            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(rm => rm.Headers).Returns(headers.Headers);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Request).Returns(requestMock.Object);

            contextMock.Object.GetUserAgent().Should().BeEquivalentTo(userAgent);
        }

        [Fact(DisplayName = "GetReferrer returns referrer header value")]
        public void GetReferrer()
        {
            var referrer = new Uri("http://SomeReferrerValue1");

            var headers = new RequestHeaders(new HeaderDictionary());
            headers.Referer = referrer;

            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(rm => rm.Headers).Returns(headers.Headers);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Request).Returns(requestMock.Object);

            contextMock.Object.GetReferrer().Should().BeEquivalentTo(referrer);
        }

        [Fact(DisplayName = "GetCorrelationId returns Accessor.CorrelationId header value")]
        public void GetCorrelationId()
        {
            var correlationId = new StringValues("CorrelationId1");

            var headers = new RequestHeaders(new HeaderDictionary());
            headers.Set(HeaderNames.CorrelationId, correlationId);

            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(rm => rm.Headers).Returns(headers.Headers);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Request).Returns(requestMock.Object);
            contextMock.Setup(cm => cm.Items).Returns(new Dictionary<object, object>());

            contextMock.Object.GetCorrelationId().Should().BeEquivalentTo(correlationId);
        }

        [Fact(DisplayName = "setCorrelationId sets the correlation id")]
        public void SetCorrelationId()
        {
            var correlationId = new StringValues("CorrelationId1");
            var items = new Dictionary<object, object>();
            var headers = new RequestHeaders(new HeaderDictionary());

            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(rm => rm.Headers).Returns(headers.Headers);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Request).Returns(requestMock.Object);
            contextMock.Setup(cm => cm.Items).Returns(items);

            contextMock.Object.SetCorrelationId(correlationId);
            contextMock.Object.GetCorrelationId().Should().BeEquivalentTo(correlationId);
        }

        [Fact(DisplayName = "GetCorrelationIdAccessor returns existing accessor")]
        public void GetCorrelationIdAccessor()
        {
            var idAccessorMock = new Mock<ICorrelationIdAccessor>();

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Items)
                .Returns(new Dictionary<object, object>() { { typeof(ICorrelationIdAccessor), idAccessorMock.Object } });

            contextMock.Object.GetCorrelationIdAccessor().Should().Be(idAccessorMock.Object);
        }

        [Fact(DisplayName = "GetCorrelationIdAccessor creates accessor with existing correlation id")]
        public void GetCorrelationIdAccessor2()
        {
            var correlationId = new StringValues("CorrelationId1");
            var items = new Dictionary<object, object>();

            var headers = new RequestHeaders(new HeaderDictionary());
            headers.Set(HeaderNames.CorrelationId, correlationId);

            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(rm => rm.Headers).Returns(headers.Headers);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Request).Returns(requestMock.Object);
            contextMock.Setup(cm => cm.Items).Returns(items);

            var accessor = contextMock.Object.GetCorrelationIdAccessor();
            accessor.Should().NotBeNull();
            accessor.CorrelationId.Should().Be(correlationId);
        }

        [Fact(DisplayName = "GetCorrelationIdAccessor creates accessor with new correlation id")]
        public void GetCorrelationIdAccessor3()
        {
            var items = new Dictionary<object, object>();
            var headers = new RequestHeaders(new HeaderDictionary());

            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(rm => rm.Headers).Returns(headers.Headers);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Request).Returns(requestMock.Object);
            contextMock.Setup(cm => cm.Items).Returns(items);

            var accessor = contextMock.Object.GetCorrelationIdAccessor();
            accessor.Should().NotBeNull();
            accessor.CorrelationId.Should().NotBeNull();
        }

        [Fact(DisplayName = "GetCorrelationIdAccessor throws on null context")]
        public void GetCorrelationIdAccessorSadPath()
        {
            HttpContext context = null;

            Action action = () => context.GetCorrelationIdAccessor();

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "GetRemoteIpAddress returns HttpContext.Connection.RemoteIpAddress")]
        public void GetRemoteIpAddress()
        {
            var remoteIpAddress = "10.0.0.1";

            var connectionMock = new Mock<ConnectionInfo>();
            connectionMock.Setup(cm => cm.RemoteIpAddress).Returns(IPAddress.Parse(remoteIpAddress));

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Connection).Returns(connectionMock.Object);

            contextMock.Object.GetRemoteIpAddress().ToString().Should().Be(remoteIpAddress);
        }

        [Fact(DisplayName = "GetForwardedFor returns X-Forwarded-For header")]
        public void GetForwardedFor()
        {
            var forwardedFor = new StringValues("SomeForwardedForValue1");

            var headers = new RequestHeaders(new HeaderDictionary());
            headers.Set(HeaderNames.ForwardedFor, forwardedFor);

            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(rm => rm.Headers).Returns(headers.Headers);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(cm => cm.Request).Returns(requestMock.Object);
            contextMock.Setup(cm => cm.Items).Returns(new Dictionary<object, object>());

            contextMock.Object.GetForwardedFor().Should().BeEquivalentTo(forwardedFor);
        }
    }
}
