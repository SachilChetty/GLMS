using GLMS.Data;
using GLMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GLMS.Services;

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

        // Inside ContractController.cs
        public IActionResult Create()
        {
            // 1. Fetch your clients from the database
            var clients = _context.Clients.ToList();

            // 2. Assign them to ViewBag so the View can see them
            ViewBag.Clients = clients;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contract contract, IFormFile file)
        {
            // 1. Tell the computer to ignore the full "Client" and "ServiceRequests" objects 
            // because we only care about the ClientId from the dropdown.
            ModelState.Remove("Client");
            ModelState.Remove("ServiceRequests");
            ModelState.Remove("FilePath"); // Set this manually below

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        contract.FilePath = await _fileService.UploadFile(file);
                    }

                    // Fallback for ServiceLevel if it's missing from the form
                    if (string.IsNullOrEmpty(contract.ServiceLevel))
                    {
                        contract.ServiceLevel = "Standard";
                    }

                    _context.Contracts.Add(contract);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Database error: " + ex.Message);
                }
            }

            // If we reach here, something failed. Refill the list!
            ViewBag.Clients = _context.Clients.ToList();
            return View(contract);
        }

        public IActionResult Download(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/pdf", fileName);
        }

        public IActionResult Index()
        {
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .ToList();

            return View(contracts);
        }

        public IActionResult Search(string status, DateTime? start, DateTime? end)
        {
            // 1. Start with the full list of contracts, including Client data for the view
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            // 2. Add Status filter if a value was provided
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status == status);
            }

            // 3. Add Start Date filter (Contracts starting on or after this date)
            if (start.HasValue)
            {
                query = query.Where(c => c.StartDate >= start.Value);
            }

            // 4. Add End Date filter (Contracts ending on or before this date)
            if (end.HasValue)
            {
                query = query.Where(c => c.EndDate <= end.Value);
            }

            // 5. Execute the query and return the filtered list to the Index view
            var results = query.ToList();
            return View("Index", results);
        }
    }
}