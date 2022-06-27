using DataAccessLayer.Mapping;
using Entities.SampleEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ContainerDAL
    {
        private SampleEntityDAL sampleEntityDAL;
        private SampleEntityDetailsDAL sampleEntityDetailsDAL;

        public IEntityDAL<SampleEntity> SampleEntityDAL
        {
            get { return sampleEntityDAL; }
        }

        public IEntityDAL<SampleEntityDetails> SampleEntityDetailsDAL
        {
            get { return sampleEntityDetailsDAL; }
        }

        public ContainerDAL()
        {
            var dataBaseDAL = new DataBaseDAL();
            var convertor = new Convertor();

            sampleEntityDetailsDAL = new SampleEntityDetailsDAL(dataBaseDAL, convertor);
            sampleEntityDAL = new SampleEntityDAL(dataBaseDAL, convertor, sampleEntityDetailsDAL);
        }
    }
}
