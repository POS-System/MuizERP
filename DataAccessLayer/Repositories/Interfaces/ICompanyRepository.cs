using DataAccessLayer.Repositories.Interfaces.Base;
using Entities.Company;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ICompanyRepository : IGetItems<Company>, ISave<Company>
    {
    }
}
