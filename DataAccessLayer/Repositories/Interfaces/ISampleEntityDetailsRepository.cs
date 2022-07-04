using DataAccessLayer.Repositories.Interfaces.Base;
using Entities.SampleEntityDetailsN;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ISampleEntityDetailsRepository : ISaveInTransaction<SampleEntityDetails>, IGetItems<SampleEntityDetails>
    {
    }
}
