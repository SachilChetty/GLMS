using Xunit;
using Microsoft.EntityFrameworkCore;
using GLMS.Controllers;
using GLMS.Data;
using GLMS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GLMS.Tests
{
    public class ContractSearchTests
    {
        [Fact]
        public void Search_ShouldFilterByStatusAndDateRange()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "SearchTestDb_" + Guid.NewGuid())
                .Options;

            using var context = new ApplicationDbContext(options);

            context.Clients.Add(new Client { Id = 1, Name = "Test", ContactDetails = "t@t.com", Region = "R" });
            context.Contracts.AddRange(
                new Contract { Id = 1, ClientId = 1, Status = "Active", StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 12, 31) },
                new Contract { Id = 2, ClientId = 1, Status = "Expired", StartDate = new DateTime(2022, 1, 1), EndDate = new DateTime(2022, 12, 31) },
                new Contract { Id = 3, ClientId = 1, Status = "Active", StartDate = new DateTime(2024, 1, 1), EndDate = new DateTime(2024, 12, 31) }
            );
            context.SaveChanges();

            var controller = new ContractController(context, null!);
            var result = controller.Search("Active", new DateTime(2023, 1, 1), new DateTime(2023, 12, 31));

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Contract>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal("Active", model.First().Status);
            Assert.Equal(1, model.First().Id);
        }
    }
}
