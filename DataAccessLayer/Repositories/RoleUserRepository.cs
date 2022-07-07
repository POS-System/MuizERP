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

        public ObservableCollection<RoleUser> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<RoleUser>();

            _dataBaseRepository.ReadCollectionWithSchema<RoleUser>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new RoleUser();
                    _roleUserMapper.Map(drd, item);
                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(RoleUser roleUser, SqlConnection conn)
        {
            _dataBaseRepository.SaveBaseItem(roleUser, conn);            
        }
    }
}

