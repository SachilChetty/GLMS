using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using GLMS.API.Data;
using GLMS.API.Models;

namespace GLMS.Tests
{
    public class ContractsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ContractsApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove every registration that touches the real DB
                    services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
                    services.RemoveAll<ApplicationDbContext>();

                    // Remove all IDbContextOptionsExtension registrations (catches SQL Server internals)
                    var dbRelated = services
                        .Where(d => d.ServiceType.FullName != null &&
                                    d.ServiceType.FullName.Contains("EntityFrameworkCore"))
                        .ToList();
                    foreach (var s in dbRelated) services.Remove(s);

                    // Re-add clean InMemory context
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb_Shared"));
                });
            });

            _client = customFactory.CreateClient();
        }

        private async Task<string> GetJwtTokenAsync()
        {
            var loginPayload = new { Username = "admin", Password = "password123" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginPayload);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return result!.Token;
        }

        private async Task AuthorizeClientAsync()
        {
            var token = await GetJwtTokenAsync();
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task GetContracts_ReturnsSuccessStatusCode()
        {
            await AuthorizeClientAsync();
            var response = await _client.GetAsync("/api/contracts");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetContracts_ReturnsNotNullJson()
        {
            await AuthorizeClientAsync();
            var response = await _client.GetAsync("/api/contracts");
            var contracts = await response.Content.ReadFromJsonAsync<List<Contract>>();
            Assert.NotNull(contracts);
        }

        [Fact]
        public async Task PostContract_ReturnsCreated()
        {
            await AuthorizeClientAsync();
            var newContract = new CreateContractDto
            {
                ClientName = "Test Client",
                Description = "Integration test contract",
                Value = 5000.00m
            };
            var response = await _client.PostAsJsonAsync("/api/contracts", newContract);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task PostContract_ThenGet_ReturnsCreatedContract()
        {
            await AuthorizeClientAsync();
            var newContract = new CreateContractDto
            {
                ClientName = "Data Integrity Client",
                Description = "Verify create then read",
                Value = 9999.99m
            };
            var postResponse = await _client.PostAsJsonAsync("/api/contracts", newContract);
            postResponse.EnsureSuccessStatusCode();
            var created = await postResponse.Content.ReadFromJsonAsync<Contract>();
            var getResponse = await _client.GetAsync($"/api/contracts/{created!.Id}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            var fetched = await getResponse.Content.ReadFromJsonAsync<Contract>();
            Assert.Equal("Data Integrity Client", fetched!.ClientName);
        }

        [Fact]
        public async Task PatchContractStatus_ReturnsOkWithUpdatedStatus()
        {
            await AuthorizeClientAsync();
            var newContract = new CreateContractDto
            {
                ClientName = "Status Test Client",
                Description = "Will be approved",
                Value = 1500.00m
            };
            var postResponse = await _client.PostAsJsonAsync("/api/contracts", newContract);
            var created = await postResponse.Content.ReadFromJsonAsync<Contract>();
            var patchPayload = new UpdateContractStatusDto { Status = "Approved" };
            var patchResponse = await _client.PatchAsJsonAsync(
                $"/api/contracts/{created!.Id}/status", patchPayload);
            Assert.Equal(HttpStatusCode.OK, patchResponse.StatusCode);
            var updated = await patchResponse.Content.ReadFromJsonAsync<Contract>();
            Assert.Equal("Approved", updated!.Status);
        }

        [Fact]
        public async Task GetContracts_WithoutToken_Returns401()
        {
            _client.DefaultRequestHeaders.Authorization = null;
            var response = await _client.GetAsync("/api/contracts");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private class TokenResponse
        {
            public string Token { get; set; } = string.Empty;
        }
    }
}

