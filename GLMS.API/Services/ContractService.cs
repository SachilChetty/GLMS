using GLMS.API.Models;
using GLMS.API.Repositories;

namespace GLMS.API.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repository;

        public ContractService(IContractRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Contract>> GetContractsAsync(string? statusFilter)
        {
            return await _repository.GetAllAsync(statusFilter);
        }

        public async Task<Contract?> GetContractByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Contract> CreateContractAsync(CreateContractDto dto)
        {
            // Business logic: map DTO to entity
            var contract = new Contract
            {
                ClientName   = dto.ClientName,
                Description  = dto.Description,
                Value        = dto.Value,
                Status       = "Pending",
                CreatedAt    = DateTime.UtcNow
            };

            return await _repository.CreateAsync(contract);
        }

        public async Task<Contract?> UpdateContractStatusAsync(int id, string status)
        {
            // Business logic: only allow valid statuses
            var allowed = new[] { "Pending", "Active", "Approved", "Declined" };
            if (!allowed.Contains(status))
                throw new ArgumentException($"Invalid status: {status}");

            return await _repository.UpdateStatusAsync(id, status);
        }
    }
}
