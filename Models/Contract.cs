
namespace GLMS.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        // Add the '?' to make these optional for validation
        public Client? Client { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Draft"; // Default value helps!
        public string? ServiceLevel { get; set; }
        public string? FilePath { get; set; }

        public List<ServiceRequest>? ServiceRequests { get; set; }
    }
}
