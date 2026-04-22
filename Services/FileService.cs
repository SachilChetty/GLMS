namespace GLMS.Services
{
    public class FileService
    {
        public async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || Path.GetExtension(file.FileName) != ".pdf")
                throw new Exception("Only PDF allowed");

            var fileName = Guid.NewGuid() + ".pdf";
            var path = Path.Combine("wwwroot/uploads", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}