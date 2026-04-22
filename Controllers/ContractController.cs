using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    public class ContractController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService;

        public ContractController(ApplicationDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public IActionResult Create()
        {
            ViewBag.Clients = _context.Clients.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contract contract, IFormFile file)
        {
            contract.FilePath = await _fileService.UploadFile(file);

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Download(string fileName)
        {
            var path = Path.Combine("wwwroot/uploads", fileName);
            var bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/pdf", fileName);
        }

        public IActionResult Index()
        {
            return View(_context.Contracts.Include(c => c.Client).ToList());
        }
    }
}
