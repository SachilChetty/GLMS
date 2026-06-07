namespace GLMS.API.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public string Status { get; set; } = "Pending";
        public string ClientName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal Value { get; set; }
    }

    // DTO used when creating a new contract (POST)
    public class CreateContractDto
    {
        public string ClientName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }

    // DTO used when updating status (PATCH)
    public class UpdateContractStatusDto
    {
        public string Status { get; set; } = string.Empty; // "Active", "Approved", "Declined"
    }
}
