using DataAccessLayer.DataReaders;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.Repositories.Interfaces;
using DataAccessLayer.Utils;
using Entities;
using Entities.Base;
using Entities.Base.Parameters;
using System.Collections.Generic;
using System.Linq;

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

            // TODO: Создать TreeBuilder в бизнес логике или клиенте
            var tree = BuildTree(result.ToList());
            tree.ResetState();

            return tree;
        }                

        private EntityCollection<MenuItem> BuildTree(IEnumerable<MenuItem> items)
        {
            var result = new EntityCollection<MenuItem>();

            var roots = GetChilds(null, items);
            foreach(var root in roots)
            {
                AddChilds(root, items);
                result.Add(root);
            }

            return result;
        }

        private IEnumerable<MenuItem> GetChilds(int? ParentID, IEnumerable<MenuItem> items)
        {
            return items.Where(i => i.ParentID == ParentID);
        }

        private void AddChilds(MenuItem item, IEnumerable<MenuItem> items)
        {
            var childs = GetChilds(item.ID, items);
            item.Childs.AddRange(childs);

            foreach (var child in childs)
                AddChilds(child, items);
        }
    }
}
