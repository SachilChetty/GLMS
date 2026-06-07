using Moq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Xunit;
using GLMS.Services;

namespace GLMS.Tests
{
    public class FileValidationTest
    {
        [Fact]
        public async Task ShouldRejectNonPdf()
        {
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(m => m.WebRootPath).Returns("C:/FakePath");
            var fileService = new FileService(mockEnv.Object);
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns("test.exe");
            await Assert.ThrowsAsync<Exception>(() =>
                fileService.UploadFile(fileMock.Object)
            );
        }
    }
}
