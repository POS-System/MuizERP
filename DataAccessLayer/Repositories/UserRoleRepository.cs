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
    internal class UserRoleRepository : IUserRoleRepository
    {
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IMapper<SqlDataReaderWithSchema, UserRole> _userRoleMapper;

        public UserRoleRepository(
            DataBaseRepository dataBaseRepository,
            IMapper<SqlDataReaderWithSchema, UserRole> userRoleMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _userRoleMapper = userRoleMapper;            
        }

        public EntityCollection<UserRole> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<UserRole>();

            _dataBaseRepository.ReadCollectionWithSchema<UserRole>(
                cmd => SqlCommandConfigurator.Configure(cmd, parameters),
                drd =>
                {
                    var item = new UserRole();
                    _userRoleMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(UserRole item, SqlConnection conn)
        {
            _dataBaseRepository.SaveBaseItem(item, conn);            
        }
    }
}

