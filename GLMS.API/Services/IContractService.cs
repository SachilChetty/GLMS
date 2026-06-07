using GLMS.API.Models;

namespace GLMS.API.Services
{
    public interface IContractService
    {
        Task<IEnumerable<Contract>> GetContractsAsync(string? statusFilter);
        Task<Contract?> GetContractByIdAsync(int id);
        Task<Contract> CreateContractAsync(CreateContractDto dto);
        Task<Contract?> UpdateContractStatusAsync(int id, string status);
    }
}
