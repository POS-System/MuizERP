using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Parameters;
using DataAccessLayer.Repositories.Interfaces;
using Entities.Base;
using Entities.Base.Parameters;
using Entities.User;
using System.Collections.ObjectModel;

namespace DataAccessLayer.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IMapper<SqlDataReaderWithSchema, BaseEntity> _baseMapper;

        public UserRepository(DataBaseRepository dataBaseRepository, IMapper<SqlDataReaderWithSchema, BaseEntity> baseMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _baseMapper = baseMapper;
        }

        public void SaveItem(User user)
        {
            _dataBaseRepository.DoInTransaction(conn => _dataBaseRepository.SaveBaseItem(user, conn));
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
                    result.Add(item);
                });

            return result;
        }
    }
}
