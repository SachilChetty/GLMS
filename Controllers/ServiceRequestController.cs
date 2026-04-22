using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ServiceRequestController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ServiceRequestService _service;
    private readonly CurrencyService _currency;

    public ServiceRequestController(
        ApplicationDbContext context,
        ServiceRequestService service,
        CurrencyService currency)
    {
        _context = context;
        _service = service;
        _currency = currency;
    }

    public IActionResult Index()
    {
        var requests = _context.ServiceRequests.ToList();
        return View(requests);
    }

    public IActionResult Create()
    {
        ViewBag.Clients = _context.Clients.ToList();

        ViewBag.Contracts = _context.Contracts.ToList();

        return View(new ServiceRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ServiceRequest request, decimal usdAmount)
    {
        // 1. Tell validation to ignore the full Contract object (we only need ContractId)
        ModelState.Remove("Contract");

        if (ModelState.IsValid)
        {
            // 2. Double-check the Status isn't null before saving
            if (string.IsNullOrEmpty(request.Status))
            {
                request.Status = "Pending";
            }

            // 3. Business logic check
            if (!_service.CanCreateRequest(request.ContractId))
            {
                ModelState.AddModelError("", "Contract must be Active");
                ViewBag.Contracts = _context.Contracts.ToList();
                return View(request);
            }

            // 4. Currency conversion
            request.CostZAR = await _currency.ConvertUsdToZar(usdAmount);

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // If we get here, validation failed. Refill the dropdowns!
        ViewBag.Contracts = _context.Contracts.ToList();
        return View(request);
    }
}