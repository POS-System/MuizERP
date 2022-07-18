using DataAccessLayer.Repositories.Interfaces.Base;
using Entities.MenuUserHistory;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserMainMenuRepository : IGetItems<UserMenuItem>, ISaveInTransaction<UserMenuItem>
    {
    }
}
