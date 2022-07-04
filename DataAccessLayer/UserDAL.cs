using DataAccessLayer.DataReaders;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using Entities.Base;
using Entities.User;
using System.Collections.ObjectModel;

namespace DataAccessLayer
{
    internal class UserDAL : IUserDAL
    {
        private DataBaseDAL _dataBaseDAL;
        private IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;

        public UserDAL(DataBaseDAL dataBaseDAL, IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseDAL = dataBaseDAL;
            _baseMapper = baseMapper;
        }

        public void SaveItem(User user)
        {
            _dataBaseDAL.DoInTransaction(conn => _dataBaseDAL.SaveBaseItem(user, conn));
        }

        public ObservableCollection<User> GetItems(IParametersContainer parametersContainer)
        {
            var result = new ObservableCollection<User>();

            _dataBaseDAL.ReadCollectionWithSchema<User>(
                sqlCmd => ParametersConfigurator.ConfigureSqlCommand(sqlCmd, parametersContainer),
                drd =>
                {
                    var item = new User();
                    _baseMapper.Map(drd, item);
                    result.Add(item);
                });

            return result;
        }

        //public void SaveItem(User user, SqlConnection conn = null)
        //{
        //    if (conn == null)
        //        _dataBaseDAL.DoInTransaction(sqlConn => _dataBaseDAL.SetBaseItem(user, sqlConn, null));
        //    else
        //        _dataBaseDAL.SetBaseItem(user, conn, null);
        //}
    }
}
