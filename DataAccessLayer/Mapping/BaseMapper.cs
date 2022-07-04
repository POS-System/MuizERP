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
    internal sealed class BaseMapper : IMapper<SqlDataReaderWithSchema, BaseEntity>
    {
        public void Map(SqlDataReaderWithSchema drd, BaseEntity currentItem)
        {
            var currentItemType = currentItem.GetType();

            // Получаем значения остальных параметров
            foreach (var property in currentItemType.GetProperties())
            {
                var loadParameter = property.GetCustomAttribute<LoadParameterAttribute>();

                // Если свойство не нуждается в загрузке значения, пропускаем его
                if (loadParameter == null) continue;

                var parameterName = loadParameter.Name ?? property.Name;
                
                ValidateDataReaderContainsRequiredField(loadParameter, parameterName, drd, currentItemType);
                
                if (!CheckDataReaderContainsField(drd, parameterName))
                {
                    if (loadParameter.DefaultValue != null)
                        property.SetValue(currentItem, loadParameter.DefaultValue, null);

                    continue;
                }

                var value = drd[parameterName];
                if (value == DBNull.Value)
                    continue;

                property.SetValue(currentItem, value, null);
            }
        }

        /// <summary>
        /// Проверка на наличие в <see cref="SqlDataReaderWithSchema"/> обязательного поля.
        /// </summary>
        /// <param name="loadParameterAttribute">Атрибут автоматического считывания данных.</param>
        /// <param name="parameterName">Название поля автоматического считывания данных.</param>
        /// <param name="drd">Объект класа <see cref="SqlDataReaderWithSchema"/>.</param>
        /// <param name="fillableItemType">Тип заполняемого объекта в <see cref="SqlDataReaderWithSchema"/>.</param>
        private static void ValidateDataReaderContainsRequiredField(LoadParameterAttribute loadParameterAttribute, string parameterName, SqlDataReaderWithSchema drd, Type fillableItemType)
        {
            if (!loadParameterAttribute.Required)
                return;
            if (!CheckDataReaderContainsField(drd, parameterName))
                throw new MappingException(string.Format("В SqlDataReader отсутствует обязательное поле '{0}'. Заполняется объект '{1}'.",
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