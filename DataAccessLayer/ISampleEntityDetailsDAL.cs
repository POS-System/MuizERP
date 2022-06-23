using Entities.SampleEntity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public interface ISampleEntityDetailsDAL
    {
        void SetSampleEntityDetails(SampleEntityDetails sampleEntityDetails, SqlConnection conn);
        IEnumerable<SampleEntityDetails> GetSampleEntityDetails(int sampleEntityID);
    }
}
