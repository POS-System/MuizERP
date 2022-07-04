using DataAccessLayer.Interfaces.Base;
using Entities.User;

namespace DataAccessLayer.Interfaces
{
    public interface IUserDAL : IGetItems<User>, ISave<User>
    { }
}
