using MuizEnums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Entities.Base
{
    /// <summary>
    /// Коллекция для объектов, наследованных от <see cref="BaseEntity"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityCollection<T> : ObservableCollection<T>, IEntityCollection<T>
        where T : BaseEntity
    {
        public EntityCollection() : base()
        {
        }

        public EntityCollection(IEnumerable<T> items) : base(items)
        {
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public void RemoveItem(T item)
        {
            if (item.State == EState.Insert)
                Remove(item);
            else
                item.State = EState.Delete;
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                RemoveItem(item);
        }

        public IEnumerable<T> GetActualItems()
        {
            return this.Where(i => i.State != EState.Delete);
        }

        public IEnumerable<T> GetSavableItems()
        {
            return this.Where(i => i.State != EState.None);
        }

        /// <summary>
        /// Старый признак изменения коллекции.
        /// </summary>
        public bool IsChangedOld
        {
            get { return this.Any(i => i.IsChangedOld); }
        }

        public bool IsModified
        {
            get {  return this.Any(i => i.IsModified || i.State == EState.Insert || i.State == EState.Delete); }
        }

        public void FixValues()
        {
            foreach (var item in this)
                item.FixValues();
        }

        public void ResetState()
        {
            foreach (var item in this)
                item.ResetState();
        }
    }
}
