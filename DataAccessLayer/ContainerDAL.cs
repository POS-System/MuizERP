using DataAccessLayer.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ContainerDAL
    {
        private SampleEntityDAL _sampleEntityDAL;
        private SampleEntityDetailsDAL _sampleEntityDetailsDAL;

        public ISampleEntityDAL SampleEntityDAL
        {
            get { return _sampleEntityDAL; }
        }

        public ISampleEntityDetailsDAL SampleEntityDetailsDAL
        {
            get { return _sampleEntityDetailsDAL; }
        }

        public ContainerDAL()
        {
            var dataBaseDAL = new DataBaseDAL();
            var baseMapper = new BaseItemFromDataReaderMapper();

            var sampleEntityDetailsDAL = _sampleEntityDetailsDAL = new SampleEntityDetailsDAL(dataBaseDAL, baseMapper);
            _sampleEntityDAL = new SampleEntityDAL(dataBaseDAL, sampleEntityDetailsDAL, baseMapper);

        }
    }
}
