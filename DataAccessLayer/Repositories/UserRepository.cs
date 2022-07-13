using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using DataAccessLayer.Repositories.Interfaces;
using Entities;
using Entities.Base;
using Entities.Base.Parameters;
using System.Collections.ObjectModel;

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

        public ObservableCollection<User> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<User>();

            _dataBaseRepository.ReadCollectionWithSchema<User>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new User();
                    _baseMapper.Map(drd, item);

                    var parameters = new ParametersContainer();
                    parameters.Add<User>(nameof(item.ID), item.ID);
                    item.UserRoles = new EntityCollection<UserRole>(_userRoleRepository.GetItems(parameters));

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
                        item.UserRoles, userRole => {
                            userRole.UserID = item.ID;
                            _userRoleRepository.SaveItem(userRole, conn);
                        });
                });
        }
    }
}
