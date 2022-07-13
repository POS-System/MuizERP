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

        public EntityCollection<UserRole> GetItems(IParametersContainer parametersContainer)
        {
            var result = new EntityCollection<UserRole>();

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

        public void SaveItem(UserRole userRole, SqlConnection conn)
        {
            _dataBaseRepository.SaveBaseItem(userRole, conn);            
        }
    }
}

