using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;
using Entities.Base.Parameters;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRoleRepository : ISaveInTransaction<UserRole>, IGetItems<UserRole>
    {        
    }
}