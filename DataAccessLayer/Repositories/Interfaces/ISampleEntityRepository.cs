using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ISampleEntityRepository : ISaveItem<SampleEntity>, IGetCollection<SampleEntity>
    {
    }
}
