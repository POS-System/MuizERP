using System;

namespace Entities.Base.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class SaveCommandAttribute : Attribute
    {

        #region Свойства

        /// <summary>
        /// Название хранимой процедуры для сохранения объекта
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Поумолчанию 60
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Названия свойств, которые игнорируются при сохранении объекта
        /// </summary>
        public string[] IgnoreProperties { get; set; }

        /// <summary>
        /// Свойство для отключения проверки на пустое название хранимой процедуры
        /// </summary>
        public bool UseEmptyCommandName { get; private set; }

        #endregion
        public SaveCommandAttribute() : this(string.Empty, false)
        { }

        public SaveCommandAttribute(string name) : this(name, false)
        {}

        public SaveCommandAttribute(bool useEmptyCommandName) : this(string.Empty, useEmptyCommandName)
        {}
     
        private SaveCommandAttribute(string name, bool useEmptyCommandName)
        {
            Name = name;
            UseEmptyCommandName = useEmptyCommandName;
            Timeout = 60;
        }

    }
}
