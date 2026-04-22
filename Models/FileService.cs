using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

public class FileService
{
    private readonly string _uploadPath;

    public FileService(IWebHostEnvironment env)
    {
        _uploadPath = Path.Combine(env.WebRootPath, "uploads");

        // Ensure folder exists
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<string> UploadFile(IFormFile file)
    {
        if (file == null || Path.GetExtension(file.FileName).ToLower() != ".pdf")
        {
            throw new Exception("Only PDF files are allowed.");
        }

        var fileName = Guid.NewGuid().ToString() + ".pdf";
        var fullPath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }
}