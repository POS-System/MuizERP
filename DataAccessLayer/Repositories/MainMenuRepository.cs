using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Parameters;
using Entities.Base.Utils;

namespace DataAccessLayer.Repositories
{
    internal class MainMenuRepository : IMainMenuRepository
    {
        private readonly DataBaseRepository _dataBaseRepository;
        private readonly IMapper<SqlDataReaderWithSchema, MenuItem> _menuItemMapper;

        public MainMenuRepository(
            DataBaseRepository dataBaseRepository,
            IMapper<SqlDataReaderWithSchema, MenuItem> menuItemMapper)
        {
            _dataBaseRepository = dataBaseRepository;
            _menuItemMapper = menuItemMapper;
        }

        public EntityCollection<MenuItem> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<MenuItem>();

            _dataBaseRepository.ReadCollectionWithSchema<MenuItem>(
                cmd => SqlCommandConfigurator.Configure(cmd, parameters),
                drd =>
                {
                    var item = new MenuItem();
                    _menuItemMapper.Map(drd, item);

                    result.Add(item);
                });

            // TODO: Строить дерево в бизнес логике или на клиенте
            var tree = EntityTreeBuilder.Build(result);

            return tree;
        }                
    }
}
