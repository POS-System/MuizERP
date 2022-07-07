using System;

namespace Entities.Base.Attributes
{

    /// <summary>
    /// Параметр автоматического считывания данных из SqlDataReader
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LoadParameterAttribute : Attribute 
    {

        #region Поля

        private string              _name;

        #endregion

        #region Свойства

        /// <summary>
        /// Название поля в SqlDataReader
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value.ToLower(); }
        }

        /// <summary>
        /// Название поля 'Name' для элементов с типом DictionaryItem
        /// </summary>
        public string DictionaryNameField { get; private set; }

        /// <summary>
        /// Индикатор поддержки null-значений
        /// Поумолчанию False
        /// </summary>
        public bool Nullable { get; set; }

        /// <summary>
        /// Свойство является значением из справочника
        /// </summary>
        public bool IsDictionaryItem { get; private set; }

        /// <summary>
        /// Признак обязательности поля
        /// По умолчанию True
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Содержит значение по умолчанию, если в дата DataReader нет данных
        /// </summary>
        public object DefaultValue { get; set; }

        #endregion

        #region Конструктор

        protected LoadParameterAttribute(bool isDictionaryItem)
        {
            IsDictionaryItem   = isDictionaryItem;
            Nullable           = false;
            Required           = true;
        }

        public LoadParameterAttribute() : this(false)
        {

        }

        /// <summary>
        /// Атрибут задает параметры свойства для метода FillFromDataReader
        /// </summary>
        /// <param name="name">Имя поля для чтения из SqlDataReader</param>
        public LoadParameterAttribute(string name) : this(false)
        {
            Name = name;
        }

        public LoadParameterAttribute(string dictionaryIdField, string dictionaryNameField) : this(true)
        {
            Name = dictionaryIdField;
            DictionaryNameField = dictionaryNameField;
        }

        #endregion

        public override string ToString()
        {
            return _name;
        }
    }
}
