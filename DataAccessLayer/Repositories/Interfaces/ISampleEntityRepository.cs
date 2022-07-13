using DataAccessLayer.Repositories.Interfaces.Base;
using Entities;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface ISampleEntityRepository : ISave<SampleEntity>, IGetItems<SampleEntity>
    {
    }


    //public interface ISimpleSampleEntityRepository : IRepository<BaseEntity>
    //{ }

    //public interface ISampleEntityRepository : IRepository<BaseEntity>, IDetailsRepository<BaseEntity>, IDetailsRepository<User>
    //{ }

    //public interface IBaseRepository
    //{ }

    //public interface IRepository<T> : ISave<T>, IGetItems<T> where T : BaseEntity
    //{ }

    //public interface IDetailsRepository<T> : ISaveInTransaction<T>, IGetItems<T> where T : BaseEntity
    //{ }
}
