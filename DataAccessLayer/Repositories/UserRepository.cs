using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Parameters;

namespace DataAccessLayer.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;

        public UserRepository(
            DataBaseRepository dataBaseRepository,
            IUserRoleRepository userRoleRepository,
            IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _userRoleRepository = userRoleRepository;
            _baseMapper = baseMapper;
        }

        public EntityCollection<User> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<User>();

            _dataBaseRepository.ReadCollectionWithSchema<User>(
                cmd => SqlCommandConfigurator.Configure(cmd, parameters),
                drd =>
                {
                    var item = new User();
                    _baseMapper.Map(drd, item);

                    var userRoleParams = new ParametersContainer();
                    userRoleParams.Add<User>(nameof(item.ID), item.ID);

                    item.UserRoles = _userRoleRepository.GetItems(userRoleParams);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(User item)
        {
            _dataBaseRepository.DoInTransaction(
                conn =>
                {
                    _dataBaseRepository.SaveBaseItem(item, conn);

                    _dataBaseRepository.SaveCollection(
                        item.UserRoles,
                        userRole =>
                        {
                            userRole.UserID = item.ID;
                            _userRoleRepository.SaveItem(userRole, conn);
                        });
                });
        }
    }
}
