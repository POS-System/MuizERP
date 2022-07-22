using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository :
        IGetItemByID<User>,
        ISaveItem<User>,
        IGetCollection<User>,
        ISaveCollection<User>
    {
    }
}
