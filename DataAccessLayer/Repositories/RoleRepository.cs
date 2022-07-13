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

        public EntityCollection<Role> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<Role>();

            _dataBaseRepository.ReadCollectionWithSchema<Role>(
                cmd => SqlCommandConfigurator.Configure(cmd, parameters),
                drd =>
                {
                    var item = new Role();
                    _baseMapper.Map(drd, item);

                    var roleUserParams = new ParametersContainer();
                    roleUserParams.Add<Role>(nameof(item.ID), item.ID);

                    item.RoleUsers = new EntityCollection<RoleUser>(_roleUserRepository.GetItems(roleUserParams));

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
