using DataAccessLayer.Repositories.Interfaces.Base;
using Entities.MenuUserHistory;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserMainMenuFavoritesRepository :
        /*IGetItems<UserMenuItem>,*/ IGetItemsByForegnKeyID<UserMenuItem>, ISaveInTransaction<UserMenuItem>
    {
    }
}
