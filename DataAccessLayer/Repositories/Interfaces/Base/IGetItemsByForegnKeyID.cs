using Entities.Base;

namespace DataAccessLayer.Repositories.Interfaces.Base
{
    public interface IGetItemsByForegnKeyID<T> where T : BaseEntity
    {
        EntityCollection<T> GetItemsByForignKey(int id);
    }
}
