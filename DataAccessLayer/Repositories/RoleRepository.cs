using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils;
using Entities.Base.Utils.Interface;

namespace DataAccessLayer.Repositories
{
    internal class RoleRepository : IRoleRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IRoleUserRepository _roleUserRepository;
        private readonly IDataMapper _dataMapper;

        public RoleRepository(
            DataRepository dataRepository,
            IRoleUserRepository roleUserRepository,
            IDataMapper dataMapper)
        {
            _dataRepository = dataRepository;
            _roleUserRepository = roleUserRepository;

            _dataMapper = dataMapper;
        }

        public EntityCollection<Role> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<Role>();

            _dataRepository.ReadCollectionWithSchema<Role>(
                cmd => cmd.ConfigureParameters(parameters),
                drd =>
                {
                    var item = new Role();
                    _dataMapper.Map(drd, item);

                    var roleUserParams = new ParametersContainer();
                    roleUserParams.Add<Role>(nameof(item.ID), item.ID);

                    item.RoleUsers = _roleUserRepository.GetItems(roleUserParams);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(Role item)
        {
            _dataRepository.DoInTransaction(
                conn =>
                {
                    _dataRepository.SaveBaseItem(item, conn);

                    _dataRepository.SaveCollection(
                        item.RoleUsers,
                        roleUser =>
                        {
                            roleUser.RoleID = item.ID;
                            _roleUserRepository.SaveItem(roleUser, conn);
                        });
                });
        }
    }
}
