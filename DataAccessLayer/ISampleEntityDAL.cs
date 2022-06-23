using Entities.SampleEntity;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface ISampleEntityDAL
    {
        void SetSampleEntity(SampleEntity sampleEntity);
        IEnumerable<SampleEntity> GetSampleEntities();
    }
}
