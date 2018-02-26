using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Carpool.API.Tests.MessageHandlers
{
    public class RequireHttpsMessageHandlerTest
    {
        [Fact]
        public async Task Returns_Forbidden_If_Request_Is_Not_Over_HTTPS()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8080");
            var requireHttpsMessageHandler = new RequireHttpsMessageHandler();

            // Act
            var response = await requireHttpsMessageHandler.InvokeAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
        
        [Fact]
        public async Task Returns_Delegated_StatusCode_When_Request_Is_Over_HTTPS()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:8080");
            var requireHttpsMessageHandler = new RequireHttpsMessageHandler();

            // Act
            var response = await requireHttpsMessageHandler.InvokeAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
