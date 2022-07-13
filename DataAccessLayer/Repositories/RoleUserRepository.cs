using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils.Interface;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories
{
    internal class RoleUserRepository : IRoleUserRepository
    {
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IMapper<SqlDataReaderWithSchema, RoleUser> _roleUserMapper;

        public RoleUserRepository(
            DataBaseRepository dataBaseRepository,
            IMapper<SqlDataReaderWithSchema, RoleUser> roleUserMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _roleUserMapper = roleUserMapper;            
        }

        public EntityCollection<RoleUser> GetItems(IParametersContainer parametersContainer)
        {
            var result = new EntityCollection<RoleUser>();

            _dataBaseRepository.ReadCollectionWithSchema<RoleUser>(
                cmd => SqlCommandConfigurator.Configure(cmd, parametersContainer),
                drd =>
                {
                    var item = new RoleUser();
                    _roleUserMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(RoleUser item, SqlConnection conn)
        {
            _dataBaseRepository.SaveBaseItem(item, conn);            
        }
    }
}

