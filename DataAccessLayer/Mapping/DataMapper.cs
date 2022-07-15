using System;
using System.Reflection;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.DataReaders;
using Entities.Base.Attributes;
using Entities.Exceptions.InnerApplicationExceptions;
using Entities.Base;

namespace DataAccessLayer.Mapping
{
    /// <summary>
    /// Класс маппер из объекта <see cref="SqlDataReaderWithSchema"/> в <see cref="BaseEntity"/>
    /// </summary>
    internal sealed class DataMapper : IDataMapper
    {
        public void Map(SqlDataReaderWithSchema drd, BaseEntity item, Action<string> fieldNameAction = null)
        {
            var type = item.GetType();

            // Получаем значения остальных параметров
            foreach (var property in type.GetProperties())
            {
                var attribute = property.GetCustomAttribute<LoadParameterAttribute>();

                // Если свойство не нуждается в загрузке значения, пропускаем его
                if (attribute == null) continue;

                var name = attribute.Name ?? property.Name;
                
                ValidateDataReaderContainsRequiredField(attribute, name, drd, type);
                
                if (!CheckDataReaderContainsField(drd, name))
                {
                    if (attribute.DefaultValue != null)
                        property.SetValue(item, attribute.DefaultValue, null);

                    continue;
                }

                if (fieldNameAction != null)
                    fieldNameAction(name);

                var value = drd[name];
                if (value == DBNull.Value)
                    continue;

                property.SetValue(item, value, null);
            }
        }

        /// <summary>
        /// Проверка на наличие в <see cref="SqlDataReaderWithSchema"/> обязательного поля.
        /// </summary>
        /// <param name="attribute">Атрибут автоматического считывания данных.</param>
        /// <param name="parameterName">Название поля автоматического считывания данных.</param>
        /// <param name="drd">Объект класа <see cref="SqlDataReaderWithSchema"/>.</param>
        /// <param name="fillableItemType">Тип заполняемого объекта в <see cref="SqlDataReaderWithSchema"/>.</param>
        private static void ValidateDataReaderContainsRequiredField(LoadParameterAttribute attribute, string parameterName, SqlDataReaderWithSchema drd, Type fillableItemType)
        {
            if (!attribute.Required)
                return;
            if (!CheckDataReaderContainsField(drd, parameterName))
                throw new MappingException(
                    string.Format(
                        "В SqlDataReader отсутствует обязательное поле '{0}'. Заполняется объект '{1}'.",
                        parameterName, fillableItemType));
        }

        /// <summary>
        /// Проверка на существование поля в <see cref="SqlDataReaderWithSchema"/>.
        /// </summary>
        /// <param name="drd">Объект класса <see cref="SqlDataReaderWithSchema"/>.</param>
        /// <param name="fieldName">Наименование поля</param>
        private static bool CheckDataReaderContainsField(SqlDataReaderWithSchema drd, string fieldName)
        {
            return drd.ContainsField(fieldName.ToLower());
        }
    }
}