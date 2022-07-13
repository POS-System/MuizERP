using Entities.Base.Attributes;
using Entities.Base.Utils;
using MuizEnums;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Entities.Base
{
    public class BaseEntity : IBaseEntity, INotifyPropertyChanged
    {
        #region Fields

        protected int _ID;
        private EState _state;
        protected int _companyID;
        protected DateTime _createDate;
        protected DateTime _modifyDate;
        protected int _createByUserID;
        protected int _modifyByUserID;
        protected byte[] _timeStamp;

        private object _fixedEntity;

        #endregion

        #region Properties

        [LoadParameter]
        [SaveParameter(Direction = ParameterDirection.InputOutput)]
        public virtual int ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                    _ID = value;

                OnPropertyChanged();
            }
        }
                
        public EState State
        {
            get { return _state; }
            set
            {
                // Если объект новый
                if (_state == EState.Insert)
                {
                    // И если хотим объект удалить
                    if (EState.Delete == value)
                        // Помечаем, чтобы не производить действий при сохранении 
                        _state = EState.None;

                    // И если не хотим сохранять
                    if (EState.None == value)
                        // Устававливаем статус, исключающий сохранение
                        _state = value;
                }
                else
                    // Устанавливаем состояние объекта
                    _state = value;
            }
        }

        [LoadParameter]
        [SaveParameter]
        public virtual int CompanyID
        {
            get { return _companyID; }
            set
            {
                if (_companyID != value)
                    _companyID = value;

                OnPropertyChanged();
            }
        }

        [LoadParameter]
        public DateTime CreateDate
        {
            get { return _createDate; }
            set
            {
                if (_createDate != value)
                    _createDate = value;

                OnPropertyChanged();
            }
        }

        [LoadParameter]
        public int CreateByUserID
        {
            get { return _createByUserID; }
            set
            {
                if (_createByUserID != value)
                    _createByUserID = value;

                OnPropertyChanged();
            }
        }

        [LoadParameter]
        public virtual DateTime ModifyDate
        {
            get { return _modifyDate; }
            set
            {
                if (_modifyDate != value)
                    _modifyDate = value;

                OnPropertyChanged();
            }
        }

        [LoadParameter]
        [SaveParameter]
        public virtual int ModifyByUserID
        {
            get { return _modifyByUserID; }
            set
            {
                if (_modifyByUserID != value)
                    _modifyByUserID = value;

                OnPropertyChanged();
            }
        }

        [LoadParameter]
        [SaveParameter(Nullable = true)]
        public virtual byte[] TimeStamp
        {
            get { return _timeStamp; }
            set
            {
                if (_timeStamp != value)
                    _timeStamp = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Признак изменения объекта.
        /// </summary>
        public bool IsModified
        {
            get
            {
                var properties = GetType().GetProperties();

                // Перебираем свойства объекта
                // Если хотя бы один из вложенных объектов изменён - считаем, что изменён весь объкет
                foreach (var property in properties)
                {
                    var type = property.PropertyType;

                    // Если поле - вложенный объект, наследующий BaseEntity
                    if (CheckIsEntity(type))
                    {
                        // У объекта BaseEntity получаем значение свойства isModified
                        var isModified = ((BaseEntity)property.GetValue(this)).IsModified;

                        if (isModified) return isModified;
                    }
                    // Если поле - коллекция EntityCollection<>
                    if (CheckIsCollection(type))
                    {
                        // Определяем тип объекта коллекции
                        // Все коллекции EntityCollection<> должны быть закрыты одним типом, наследованным от BaseEntity
                        // Если они закрыты иным типом - произойдёт ошибка
                        var entityType = type.GetGenericArguments()
                            .Where(a => typeof(BaseEntity).IsAssignableFrom(a))
                            .Single();

                        // На основе найденного типа объекта коллекции формируем дженерик тип коллекции объектов
                        var collectionType = typeof(EntityCollection<>).MakeGenericType(entityType);

                        var isModifiedProperty = collectionType.GetProperty("IsModified");

                        // Если у коллекции нет свойства IsModified
                        if (isModifiedProperty == null) continue;

                        // Получаем объект EntityCollection<>
                        var collection = property.GetValue(this);

                        // У объекта EntityCollection<> получаем значение свойства isModified
                        var isModified = (bool)isModifiedProperty.GetValue(collection);

                        if (isModified) return isModified;
                    }
                }

                return _state != EState.None;
            }
        }

        /// <summary>
        /// Новый признак изменения объекта.
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (_fixedEntity == null) return false;

                // Сравниваем приватные поля объектов

                var fixedFields = _fixedEntity.GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                var currentFields = GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                
                foreach (var currentField in currentFields)
                {
                    var currentValue = currentField.GetValue(this);

                    // Если поле - вложенный объект, наследующий BaseEntity
                    var entity = currentValue as IBaseEntity;
                    if (entity != null)
                    {
                        if (entity.IsChanged) return entity.IsChanged;
                        else continue;
                    }

                    // Если поле - коллекция EntityCollection<>
                    var collection = currentValue as IEntityCollection;
                    if (collection != null)
                    {
                        if (collection.IsChanged) return collection.IsChanged;
                        else continue;
                    }

                    var fixedField = fixedFields
                        .Where(fp => fp.Name == currentField.Name)
                        .Single();

                    var fixedValue = fixedField.GetValue(_fixedEntity);

                    if (currentValue is null && fixedValue is null) continue;

                    // Если свойство может быть null - проверяем изменение
                    if (currentValue is null && !(fixedValue is null) ||
                        fixedValue is null && !(currentValue is null)) return true;

                    if (!currentValue.Equals(fixedValue)) return true;
                }

                return _state == EState.Insert || _state == EState.Delete;
            }
        }

        #endregion

        #region Fixed Value

        public void FixValues()
        {
            var type = GetType();
            _fixedEntity = Activator.CreateInstance(type);

            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var value = field.GetValue(this);

                // Если поле - вложенный объект, наследующий BaseEntity
                var entity = value as IBaseEntity;
                if (entity != null)
                {
                    entity.FixValues();
                    continue;
                }

                // Если поле - коллекция EntityCollection<>
                var collection = value as IEntityCollection;
                if (collection != null)
                {
                    collection.FixValues();
                    continue;
                }

                // Если поле значимого типа
                field.SetValue(_fixedEntity, value);
            }
        }

        /// <summary>
        /// Сбрасывает состояние объекта и вложенных объектов в None.
        /// </summary>
        public void ResetState()
        {
            var properties = GetType().GetProperties()
                .Where(p => p.CanWrite);

            foreach (var property in properties)
            {
                var value = property.GetValue(this);

                var entity = value as IBaseEntity;
                if (entity != null)
                {
                    entity.ResetState();
                    continue;
                }

                var collection = value as IEntityCollection;
                if (collection != null)
                {
                    collection.ResetState();
                    continue;
                }
            }

            _state = EState.None;
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // Если объект удаляется - больше не меняем его состояние
            if (State != EState.Delete)
                State = EState.Update;
        }

        #endregion

        /// <summary>
        /// Определяет, является ли тип наследником BaseEntity.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool CheckIsEntity(Type type)
        {
            var isEntity = typeof(BaseEntity).IsAssignableFrom(type);

            return isEntity;
        }

        /// <summary>
        /// Определяет, является ли тип коллекцией EntityCollection<>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool CheckIsCollection(Type type)
        {
            var isCollection =
                    type.IsGenericType &&
                    type.GetGenericTypeDefinition() == typeof(EntityCollection<>);

            return isCollection;
        }
    }
}
