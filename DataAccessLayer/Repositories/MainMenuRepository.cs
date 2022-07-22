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
        private readonly IDataMapper _dataMapper;

        public MainMenuRepository(
            DataRepository dataRepository,
            IDataMapper dataMapper)
        {
            _dataRepository = dataRepository;

            _dataMapper = dataMapper;
        }

        public EntityCollection<MenuItem> GetCollection(IParametersContainer parameters)
        {
            var result = new EntityCollection<MenuItem>();

            _dataRepository.ReadCollectionWithSchema<MenuItem>(
                cmd =>
                {
                    cmd.CommandText = "xp_GetMainMenu";
                    cmd.AddParameters(parameters);
                },
                drd =>
                {
                    var item = new MenuItem();
                    _dataMapper.Map(drd, item);

                    result.Add(item);
                });

            // TODO: Строить дерево в бизнес логике или на клиенте
            var tree = EntityTreeBuilder.Build(result);

            return tree;
        }                
    }
}
