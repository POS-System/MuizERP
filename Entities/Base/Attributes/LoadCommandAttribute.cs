using System;

namespace Entities.Base.Attributes
{

    [AttributeUsage(AttributeTargets.Class)]
    public class LoadCommandAttribute : Attribute
    {


        #region Свойства

        /// <summary>
        /// Название ключевого поля в SqlDataReader
        /// </summary>
        public string IdFieldName { get; private set; }

        /// <summary>
        /// Название поля временной версии объекта в SqlDataReader, дополняещего ключ 
        /// </summary>
        public string VersionDateFieldName { get; private set; }

        #endregion

        /// <summary>
        /// Класс параметров для чтения данных из SqlDataReader
        /// </summary>
        /// <param name="idFieldName">Название ключевого поля в SqlDataReader</param>
        /// <param name="versionDateFieldName">Название поля временной версии объекта в SqlDataReader, дополняещего ключ (По умолчанию "begin_date")</param>
        public LoadCommandAttribute(string idFieldName, string versionDateFieldName = "begin_date")
        {
            IdFieldName = idFieldName;
            VersionDateFieldName = versionDateFieldName;
        }

    }
}
