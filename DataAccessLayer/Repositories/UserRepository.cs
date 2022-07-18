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
        private readonly IUserMainMenuRepository _menuFavoritesRepository;
        private readonly IUserMainMenuRepository _menuHistoryRepository;

        private readonly IDataMapper _dataMapper;

        public UserRepository(
            DataRepository dataRepository,
            IUserRoleRepository userRoleRepository,
            IUserMainMenuRepository menuFavoritesRepository,
            IUserMainMenuRepository menuHistoryRepository,
            IDataMapper dataMapper)
        {
            _dataRepository = dataRepository;
            _userRoleRepository = userRoleRepository;
            _menuFavoritesRepository = menuFavoritesRepository;
            _menuHistoryRepository = menuHistoryRepository;

            _dataMapper = dataMapper;
        }

        public User GetItemById(int id)
        {
            return _dataRepository.GetSingleItem(
                cmd => cmd.AddIdentifier(id),
                drd =>
                {
                    var item = new User();
                    _dataMapper.Map(drd, item);

                    var parameters = new ParametersContainer();
                    parameters.Add<User>(nameof(item.ID), item.ID);

                    item.UserRoles = _userRoleRepository.GetItems(parameters);
                    item.MenuFavorites = _menuFavoritesRepository.GetItems(parameters);
                    item.MenuHistory = _menuHistoryRepository.GetItems(parameters);

                    item.Fix();
                    item.ResetState();

                    return item;
                });
        }

        public EntityCollection<User> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<User>();

            _dataRepository.ReadCollectionWithSchema<User>(
                cmd => cmd.AddParameters(parameters),
                drd =>
                {
                    var item = new User();
                    _dataMapper.Map(drd, item);

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

                    _dataRepository.SaveCollection(
                        item.MenuFavorites,
                        userFavorites =>
                        {
                            userFavorites.UserID = item.ID;
                            _menuFavoritesRepository.SaveItem(userFavorites, conn);
                        });

                    _dataRepository.SaveCollection(
                        item.MenuHistory,
                        userHistory =>
                        {
                            userHistory.UserID = item.ID;
                            _menuHistoryRepository.SaveItem(userHistory, conn);
                        });

                    item.ResetState();
                    item.Fix();
                });
        }
    }
}
