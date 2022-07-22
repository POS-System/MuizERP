using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ICompanyRepository : IGetCollection<Company>, ISaveItem<Company>
    {
    }
}
