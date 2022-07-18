using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository : IGetItemById<User>, IGetItems<User>, ISave<User>
    {
    }
}
