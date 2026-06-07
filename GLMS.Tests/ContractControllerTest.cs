using Xunit;
using Microsoft.EntityFrameworkCore;
using GLMS.Controllers;
using GLMS.Data;
using GLMS.Models;
using Microsoft.AspNetCore.Mvc;

public class ContractControllerTests
{
    [Fact]
    public void Index_ReturnsAViewResult_WithAListOfContracts()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_Controller_" + Guid.NewGuid())
            .Options;

        using var context = new ApplicationDbContext(options);

        context.Clients.Add(new Client
        {
            Id = 1,
            Name = "Test Client",
            ContactDetails = "test@test.com",
            Region = "Test Region"
        });
        context.Contracts.Add(new Contract
        {
            Id = 1,
            ClientId = 1,
            Status = "Active",
            ServiceLevel = "Standard",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(1)
        });
        context.SaveChanges();

        var controller = new ContractController(context, null!);
        var result = controller.Index();
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Contract>>(viewResult.ViewData.Model);
        Assert.Single(model);
    }
}
