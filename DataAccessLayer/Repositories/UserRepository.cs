using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils.ParametersContainers;

namespace DataAccessLayer.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserMainMenuFavoritesRepository _favoritesRepository;
        private readonly IUserMainMenuHistoryRepository _historyRepository;

        private readonly IDataMapper _dataMapper;

        public UserRepository(
            DataRepository dataRepository,
            IUserRoleRepository userRoleRepository,
            IUserMainMenuFavoritesRepository favoritesRepository,
            IUserMainMenuHistoryRepository historyRepository,
            IDataMapper dataMapper)
        {
            _dataRepository = dataRepository;
            _userRoleRepository = userRoleRepository;
            _favoritesRepository = favoritesRepository;
            _historyRepository = historyRepository;

            _dataMapper = dataMapper;
        }

        public User GetItemByID(int id)
        {
            return _dataRepository.GetSingleItem(
                cmd => cmd.AddIdentifier(id),
                drd =>
                {
                    var item = new User();
                    _dataMapper.Map(drd, item);

                    //var parameters = new ParametersContainer();
                    //parameters.Add<User>(nameof(item.ID), item.ID);

                    item.Settings = GetUserSettingsByUserID(id);

                    //item.UserRoles = _userRoleRepository.GetItems(parameters);
                    //item.MenuFavorites = _favoritesRepository.GetItems(parameters);
                    //item.MenuHistory = _historyRepository.GetItems(parameters);
                    item.UserRoles = _userRoleRepository.GetItemsByUserID(item.ID);
                    item.MenuFavorites = _favoritesRepository.GetItemsByForignKey(item.ID);
                    item.MenuHistory = _historyRepository.GetItemsByForignKey(item.ID);

                    item.ResetState();
                    item.Fix();

                    return item;
                });
        }

        public EntityCollection<User> GetCollection(IParametersContainer parameters)
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

                    item.Settings.UserID = item.ID;
                    _dataRepository.SaveBaseItem(item.Settings, conn);

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
                            _favoritesRepository.SaveItem(userFavorites, conn);
                        });

                    _dataRepository.SaveCollection(
                        item.MenuHistory,
                        userHistory =>
                        {
                            userHistory.UserID = item.ID;
                            _historyRepository.SaveItem(userHistory, conn);
                        });

                    item.ResetState();
                    item.Fix();
                });
        }

        private UserSettings GetUserSettingsByUserID(int id)
        {
            return _dataRepository.GetSingleItemOrDefault(
                cmd => cmd.AddForignKey<User>(id),
                drd =>
                {
                    var item = new UserSettings();
                    _dataMapper.Map(drd, item);

                    return item;
                });
        }

        public void SaveCollection(IEntityCollection<User> collection)
        {
            _dataRepository.SaveCollectionWithDataTable(collection);
        }
    }
}
