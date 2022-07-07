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
        private readonly IRoleUserRepository _roleUserRepository;
        private readonly IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;        

        public RoleRepository(
            DataBaseRepository dataBaseRepository,
            IRoleUserRepository roleUserRepository,
            IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _roleUserRepository = roleUserRepository;
            _baseMapper = baseMapper;            
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

                    var parameters = new ParametersContainer();
                    parameters.Add<Role>(nameof(item.ID), item.ID);
                    item.RoleUsers = _roleUserRepository.GetItems(parameters);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(Role item)
        {
            _dataBaseRepository.DoInTransaction(
                conn =>
                {
                    _dataBaseRepository.SaveBaseItem(item, conn);

                    _dataBaseRepository.SaveCollection(
                        item.RoleUsers, detail => {
                            detail.RoleID = item.ID;
                            _roleUserRepository.SaveItem(detail, conn);
                        });
                });
        }
    }
}
