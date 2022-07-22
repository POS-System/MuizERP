using DataAccessLayer.Repositories.Interfaces.Base;
using Entities.MenuUserHistory;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserMainMenuHistoryRepository :
        /*IGetItems<UserMenuItem>,*/ IGetItemsByForegnKeyID<UserMenuItem>, ISaveItemInTransaction<UserMenuItem>
    {
    }
}
