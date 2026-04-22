namespace GLMS.Services
{
    public class FileService
    {
        private readonly string _uploadPath;

        public FileService(IWebHostEnvironment env)
        {
            _uploadPath = Path.Combine(env.WebRootPath, "uploads");
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || Path.GetExtension(file.FileName).ToLower() != ".pdf")
                throw new Exception("Only PDF files allowed");

            var fileName = Guid.NewGuid() + ".pdf";
            var path = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
