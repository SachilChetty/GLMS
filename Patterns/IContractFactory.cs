using GLMS.Models;

namespace GLMS.Patterns
{
    public interface IContractFactory
    {
        Contract CreateContract(string type);
    }

    public class ContractFactory : IContractFactory
    {
        public Contract CreateContract(string type)
        {
            return type switch
            {
                "Active" => new Contract { Status = "Active" },
                "Expired" => new Contract { Status = "Expired" },
                _ => new Contract { Status = "Draft" }
            };
        }
    }
}
