using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using DataAccessLayer.Repositories.Interfaces;
using Entities;
using Entities.Base;
using Entities.Base.Parameters;
using Entities.User;
using System.Collections.ObjectModel;

namespace DataAccessLayer.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;

        public UserRepository(
            DataBaseRepository dataBaseRepository,
            IRoleRepository roleRepository,
            IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _roleRepository = roleRepository;
            _baseMapper = baseMapper;
        }

        public ObservableCollection<User> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<User>();

            _dataBaseRepository.ReadCollectionWithSchema<User>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new User();
                    _baseMapper.Map(drd, item);

                    var roleParameters = new ParametersContainer();
                    roleParameters.Add<User>(nameof(item.ID), item.ID);
                    item.UserRoles = _roleRepository.GetUserRoles(roleParameters);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(User user)
        {
            _dataBaseRepository.DoInTransaction(
                conn =>
                {
                    _dataBaseRepository.SaveBaseItem(user, conn);

                    _dataBaseRepository.SaveCollection(
                        user.UserRoles, userRole => {
                            userRole.UserID = user.ID;
                            _roleRepository.SaveUserRole(userRole, conn);
                        });
                });
        }
    }
}
