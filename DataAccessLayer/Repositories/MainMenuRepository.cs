using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Utils;
using Entities.Base.Utils.Interface;

namespace DataAccessLayer.Repositories
{
    internal class MainMenuRepository : IMainMenuRepository
    {
        private readonly DataRepository _dataRepository;
        private readonly IMapper<SqlDataReaderWithSchema, MenuItem> _menuItemMapper;

        public MainMenuRepository(
            DataRepository dataRepository,
            IMapper<SqlDataReaderWithSchema, MenuItem> menuItemMapper)
        {
            _dataRepository = dataRepository;

            _menuItemMapper = menuItemMapper;
        }

        public EntityCollection<MenuItem> GetItems(IParametersContainer parameters)
        {
            var result = new EntityCollection<MenuItem>();

            _dataRepository.ReadCollectionWithSchema<MenuItem>(
                cmd =>
                {
                    cmd.CommandText = "xp_GetMeinMenu";
                    cmd.AddParameters(parameters);
                },
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
