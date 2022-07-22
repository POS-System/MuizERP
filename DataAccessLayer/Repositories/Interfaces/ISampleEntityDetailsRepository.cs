using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ISampleEntityDetailsRepository : ISaveItemInTransaction<SampleEntityDetails>, IGetCollection<SampleEntityDetails>
    {
    }
}
