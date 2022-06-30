using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using Entities.Base;
using Entities.Company;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class CompanyDAL : IEntityDAL<Company>
    {
        private DataBaseDAL _dataBaseDAL;
        private IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;

        public CompanyDAL(
            DataBaseDAL dataBaseDAL,
            IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseDAL = dataBaseDAL;
            _baseMapper = baseMapper;
        }

        public ObservableCollection<Company> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<Company>();

            _dataBaseDAL.ReadCollectionWithSchema<Company>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new Company();
                    _baseMapper.Map(drd, item);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(Company entity, SqlConnection conn = null)
        {
            throw new NotImplementedException();
        }
    }
}
