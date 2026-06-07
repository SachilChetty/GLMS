using Xunit;
using Microsoft.EntityFrameworkCore;
using GLMS.Data;
using GLMS.Models;
using GLMS.Services;

namespace GLMS.Tests
{
    public class ServiceRequestTest
    {
        [Fact]
        public void ShouldNotAllowExpiredContract()
        {
            // 1. ARRANGE: Create the 'mockContext' using an In-Memory Database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ServiceRequestTestDb_" + Guid.NewGuid().ToString())
                .Options;

            using (var mockContext = new ApplicationDbContext(options))
            {
                // Define the 'expiredContractId' by actually adding one to the DB
                var expiredContractId = 101;

                mockContext.Contracts.Add(new Contract
                {
                    Id = expiredContractId,
                    Status = "Expired",
                    // Add any other required fields for your model here
                    StartDate = DateTime.Now.AddMonths(-2),
                    EndDate = DateTime.Now.AddMonths(-1)
                });
                mockContext.SaveChanges();

                // Initialize the service with the mock context
                var service = new ServiceRequestService(mockContext);

                // 2. ACT
                var result = service.CanCreateRequest(expiredContractId);

                // 3. ASSERT
                Assert.False(result);
            }
        }
    }
}