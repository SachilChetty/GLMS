using GLMS.Data;
using GLMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clients = _context.Clients.ToList();
            return View(clients);
        }

        public IActionResult Create()
        {
            return View(new Client());
        }

        [HttpPost]
        public IActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Add(client);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }
    }
}
