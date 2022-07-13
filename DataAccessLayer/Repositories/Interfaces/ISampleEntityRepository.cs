using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ISampleEntityRepository : ISave<SampleEntity>, IGetItems<SampleEntity>
    {
    }
}
