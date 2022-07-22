using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IRoleUserRepository : ISaveItemInTransaction<RoleUser>, IGetCollection<RoleUser>
    {
    }
}
