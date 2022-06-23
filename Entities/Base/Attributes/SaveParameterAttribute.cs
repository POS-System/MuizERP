using System;
using System.Data;

namespace Entities.Base.Attributes
{
 
    [AttributeUsage(AttributeTargets.Property)]
    public class SaveParameterAttribute : Attribute
    {

        #region Поля

        private string              _name;
        private int                 _size;
        private ParameterDirection  _direction;
        private bool                _nullable;
        private object              _nullValue;
        #endregion

        #region Свойства

        /// <summary>
        /// Название SQL параметра
        /// При пустом значении, используется имя совйства объекта
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Размер поля, применяется если больше 0
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Направление передачи значения параметра
        /// По умолчанию ParameterDirection.Input
        /// </summary>
        public ParameterDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// Индикатор поддержки null-значений
        /// Поумолчанию False
        /// </summary>
        public bool Nullable
        {
            get { return _nullable; }
            set { _nullable = value; }
        }

        /// <summary>
        /// Значение заменяемое на DBNull.Value
        /// </summary>
        public object NullValue
        {
            get { return _nullValue; }
            set { _nullValue = value; }
        }

        #endregion

        #region Конструктор
        public SaveParameterAttribute()
        {
            _name = string.Empty;
            _size = 0;
            _direction = ParameterDirection.Input;
            _nullable = false;
            _nullValue = null;
        } 
        #endregion

        public override string ToString()
        {
            return _name;
        }

    }
}
