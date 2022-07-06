using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using DataAccessLayer.Repositories.Interfaces;
using Entities;
using Entities.Base;
using Entities.Base.Parameters;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;
        private readonly IMapper<SqlDataReaderWithSchema, UserRole> _userRoleMapper;

        public RoleRepository(
            DataBaseRepository dataBaseRepository,
            IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper,
            IMapper<SqlDataReaderWithSchema, UserRole> userRoleMapper)
        {
            _dataBaseRepository = dataBaseRepository;

            _baseMapper = baseMapper;
            _userRoleMapper = userRoleMapper;
        }

        public ObservableCollection<Role> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<Role>();

            _dataBaseRepository.ReadCollectionWithSchema<Role>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new Role();
                    _baseMapper.Map(drd, item);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(Role item)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<UserRole> GetUserRoles(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<UserRole>();

            _dataBaseRepository.ReadCollectionWithSchema<UserRole>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new UserRole();
                    _userRoleMapper.Map(drd, item);
                    result.Add(item);
                });

            return result;
        }

        public void SaveUserRole(UserRole userRole, SqlConnection conn)
        {
            _dataBaseRepository.SaveBaseItem(userRole, conn);
        }
    }
}
