using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.MenuUserHistory;
using System.Data.SqlClient;

namespace DataAccessLayer.Repositories
{
    internal class UserMainMenuFavoritesRepository : IUserMainMenuFavoritesRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IMapper<SqlDataReaderWithSchema, UserMenuItem> _userMenuItemMapper;

        public UserMainMenuFavoritesRepository(
            DataRepository dataRepository,
            IMapper<SqlDataReaderWithSchema, UserMenuItem> userMenuItemMapper)
        {
            _dataRepository = dataRepository;
            _userMenuItemMapper = userMenuItemMapper;
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
                    _userMenuItemMapper.Map(drd, item);

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
