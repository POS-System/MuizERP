using System;

namespace Entities.Base.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class LoadCommandAttribute : Attribute
    {
        #region Свойства  
        /// <summary>
        /// Название хранимой процедуры для сохранения объекта
        /// </summary>
        public string Name { get; private set; }        

        /// <summary>
        /// Свойство для отключения проверки на пустое название хранимой процедуры
        /// </summary>
        public bool UseEmptyCommandName { get; private set; }

        #endregion
        public LoadCommandAttribute() : this(string.Empty, false)
        {
        }

        public LoadCommandAttribute(string name) : this(name, false)
        {
        }

        /// <summary>
        /// Класс параметров для чтения данных из SqlDataReader
        /// </summary>
        public LoadCommandAttribute(string name, bool useEmptyCommandName)
        {
            Name = name;
            UseEmptyCommandName = useEmptyCommandName;
        }
    }
}
