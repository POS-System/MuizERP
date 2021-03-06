using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;
using Entities.Base;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRoleRepository :
        ISaveItemInTransaction<UserRole>/*, IGetItems<UserRole>, IGetItemsByForegnKeyID<UserRole>*/
    {
        EntityCollection<UserRole> GetItemsByUserID(int userID);
    }
}