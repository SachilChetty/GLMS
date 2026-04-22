using GLMS.Data;

namespace GLMS.Services
{
    public class ServiceRequestService
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CanCreateRequest(int contractId)
        {
            var contract = _context.Contracts.Find(contractId);

            return contract != null && contract.Status == "Active";
        }
    }
}
