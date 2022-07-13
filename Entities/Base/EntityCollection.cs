﻿using MuizEnums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Entities.Base
{
    /// <summary>
    /// Коллекция для объектов, наследованных от <see cref="BaseEntity"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityCollection<T> : ObservableCollection<T>, IEntityCollection
        where T : BaseEntity
    {
        public EntityCollection() : base()
        {
        }

        public EntityCollection(IEnumerable<T> items) : base(items)
        {
        }

        /// <summary>
        /// Удаляет объект.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(T item)
        {
            if (item.State == EState.Insert)
                Remove(item);
            else
                item.State = EState.Delete;
        }

        /// <summary>
        /// Удаляет объекты, переданные списком.
        /// </summary>
        /// <param name="items"></param>
        public void RemoveRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                RemoveItem(item);
        }

        /// <summary>
        /// Возвращает объекты, не помеченные на удаление.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetActualItems()
        {
            return this.Where(i => i.State != EState.Delete);
        }

        /// <summary>
        /// Возвращает объекты, сохраняемые в БД.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetSavableItems()
        {
            return this.Where(i => i.State != EState.None);
        }

        /// <summary>
        /// Признак изменения коллекции.
        /// </summary>
        public bool IsModified
        {
            get { return this.Any(i => i.IsModified); }
        }

        /// <summary>
        /// Новый признак изменения коллекции.
        /// </summary>
        public bool IsChanged
        {
            get {  return this.Any(i => i.IsChanged || i.State == EState.Insert || i.State == EState.Delete); }
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