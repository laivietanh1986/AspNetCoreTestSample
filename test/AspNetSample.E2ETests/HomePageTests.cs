using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using AspNetCoreSample;
using System;
using System.Collections.Generic;
using AspNetSample.E2ETests.Helpers;
using AngleSharp.Dom.Html;

namespace AspNetSample.E2ETests
{
    public class HomePageTests : IClassFixture<WebApplicationFactory<Program>>
    {
        public HomePageTests(WebApplicationFactory<Program> factory)
        {
            Factory = factory;
        }

        public WebApplicationFactory<Program> Factory { get; }

        [Fact]
        public async Task CanRetrieveHomePageAsync()
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/About")]
        [InlineData("/Home/Contact")]
        [InlineData("/Identity/Account/Login")]
        [InlineData("/Identity/Account/Register")]
        public async Task CanGetApplicationEndpoints(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task ManagingUsersRequiresAnAuthenticatedUser()
        {
            // Arrange
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost/"),
                AllowAutoRedirect = false
            });

            // Act
            var response = await client.GetAsync("/Identity/Account/Manage");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("https://localhost/Identity/Account/Login", response.Headers.Location.ToString());
        }

        [Fact]
        public async Task CanLogin()
        {
            var client = Factory.CreateClient();

            // Act
            var loginPage = await client.GetAsync("/Identity/Account/Login");
            Assert.Equal(HttpStatusCode.OK, loginPage.StatusCode);
            var loginPageHtml = await HtmlHelpers.GetDocumentAsync(loginPage);

            var profileWithUserName = await client.SendAsync(
                (IHtmlFormElement)loginPageHtml.QuerySelector("#account"),
                new Dictionary<string, string> { ["Input_Email"] = "thienn@outlook.com", ["Input_Password"] = "1qazZAQ!" });

            Assert.Equal(HttpStatusCode.OK, profileWithUserName.StatusCode);
            var profileWithUserHtml = await HtmlHelpers.GetDocumentAsync(profileWithUserName);
            var userLogin = profileWithUserHtml.QuerySelector("#logoutForm a");
            Assert.Equal("Hello thienn@outlook.com!", userLogin.TextContent);
        }
    }
}
