using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRoleRepository : ISaveInTransaction<UserRole>, IGetItems<UserRole>
    {        
    }
}