using System.Collections.Generic;
using System.Linq;

namespace Entities.Base.Utils
{
    public static class EntityTreeBuilder
    {
        public static EntityCollection<T> Build<T>(IEnumerable<T> items)
            where T : BaseTreeEntity
        {
            var result = new EntityCollection<T>();

            var roots = GetChilds(null, items);
            foreach (var root in roots)
            {
                AddChilds(root, items);
                result.Add(root);
            }

            result.ResetState();
            result.Fix();

            return result;
        }

        private static IEnumerable<T> GetChilds<T>(int? ParentID, IEnumerable<T> items)
            where T : BaseTreeEntity
        {
            return items.Where(i => i.ParentID == ParentID);
        }

        private static void AddChilds<T>(T item, IEnumerable<T> items)
            where T : BaseTreeEntity
        {
            var childs = GetChilds(item.ID, items);
            item.Childs.AddRange(childs);

            foreach (var child in childs)
                AddChilds(child, items);
        }
    }
}
