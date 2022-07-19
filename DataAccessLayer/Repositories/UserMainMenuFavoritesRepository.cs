using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils.Interface;
using Entities.MenuUserHistory;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories
{
    internal class UserMainMenuFavoritesRepository : IUserMainMenuFavoritesRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IDataMapper _dataMapper;

        public UserMainMenuFavoritesRepository(
            DataRepository dataRepository,
            IDataMapper dataMapper)
        {
            _dataRepository = dataRepository;
            _dataMapper = dataMapper;
        }

        /*
        public EntityCollection<UserMenuItem> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<UserMenuItem>();

            _dataRepository.ReadCollectionWithSchema<UserMenuItem>(
                cmd =>
                {
                    cmd.CommandText = "xp_GetUserMainMenuFavorites";
                    cmd.AddParameters(parameters);
                },
                drd =>
                {
                    var item = new UserMenuItem();
                    _dataMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }
        */

        public EntityCollection<UserMenuItem> GetItemsByForignKey(int id)
        {
            var result = new EntityCollection<UserMenuItem>();

            _dataRepository.ReadCollectionWithSchema<UserMenuItem>(
                cmd =>
                {
                    cmd.CommandText = "xp_GetUserMainMenuFavorites";
                    cmd.AddForignKey<User>(id);
                },
                drd =>
                {
                    var item = new UserMenuItem();
                    _dataMapper.Map(drd, item);

                    result.Add(item);
                });

            return result;
        }

        public void SaveItem(UserMenuItem item, SqlConnection conn)
        {
            _dataRepository.SaveBaseItem(item, conn,
                cmd => cmd.CommandText = "xp_SaveUserMainMenuFavorites");
        }
    }
}
