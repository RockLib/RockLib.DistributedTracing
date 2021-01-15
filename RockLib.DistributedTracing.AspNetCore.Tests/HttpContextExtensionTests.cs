using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace RockLib.DistributedTracing.AspNetCore.Tests
{
    public class HttpContextExtensionTests
    {
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

        [Fact(DisplayName = "SetCorrelationId sets the correlation id")]
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
    }
}
