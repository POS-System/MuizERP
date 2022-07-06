using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ICompanyRepository : IGetItems<Company>, ISave<Company>
    {
    }
}
