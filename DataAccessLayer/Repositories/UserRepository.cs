using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils;
using Entities.Base.Utils.Interface;

namespace DataAccessLayer.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IDataMapper _dataMapper;

        public UserRepository(
            DataRepository dataRepository,
            IUserRoleRepository userRoleRepository,
            IDataMapper dataMapper)
        {
            _dataRepository = dataRepository;
            _userRoleRepository = userRoleRepository;

            _dataMapper = dataMapper;
        }

        public EntityCollection<User> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<User>();

            _dataRepository.ReadCollectionWithSchema<User>(
                cmd => cmd.ConfigureParameters(parameters),
                drd =>
                {
                    var item = new User();
                    _dataMapper.Map(drd, item);

                    var userRoleParams = new ParametersContainer();
                    userRoleParams.Add<User>(nameof(item.ID), item.ID);

                    item.UserRoles = _userRoleRepository.GetItems(userRoleParams);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(User item)
        {
            _dataRepository.DoInTransaction(
                conn =>
                {
                    _dataRepository.SaveBaseItem(item, conn);

                    _dataRepository.SaveCollection(
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
