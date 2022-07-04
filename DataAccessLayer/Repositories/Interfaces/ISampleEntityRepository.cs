using DataAccessLayer.Repositories.Interfaces.Base;
using Entities.SampleEntity;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ISampleEntityRepository : ISave<SampleEntity>, IGetItems<SampleEntity>
    {
    }
}
