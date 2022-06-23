using System;
using System.Reflection;
using DataAccessLayer.Mapping.Interface;
using DataAccessLayer.DataReaders;
using Entities.Base.Attributes;
using Entities.Exceptions.InnerApplicationExceptions;

namespace DataAccessLayer.Mapping
{
    /// <summary>
    /// Класс маппер из объекта <see cref="SqlDataReaderWithSchema"/> в <see cref="object"/>
    /// </summary>
    internal sealed class BaseItemFromDataReaderMapper : IMapper<SqlDataReaderWithSchema, object>
    {
        public void Map(SqlDataReaderWithSchema drd, object baseItem)
        {
            Map(drd, baseItem, baseItem.GetType());
        }

        private void Map(SqlDataReaderWithSchema drd, object currentItem, Type fillableItemType, bool required = true)
        {
            //Получаем значение ключевого поля
            var currentItemType = currentItem.GetType();
            var loadCommand = currentItemType.GetCustomAttribute<LoadCommandAttribute>();

            ValidateLoadCommand(loadCommand, currentItemType);
            if (required)
                ValidateDataReaderContainsIdField(drd, loadCommand.IdFieldName, currentItemType, fillableItemType);
            else if (!CheckDataReaderContainsField(drd, loadCommand.IdFieldName))
                return;

            //Получаем значения остальных параметров
            foreach (var property in currentItemType.GetProperties())
            {
                var loadParameter = property.GetCustomAttribute<LoadParameterAttribute>();
                // если свойство не нуждается в загрузке значения, пропускаем его
                if (loadParameter == null) continue;

                //ValidateFieldNameNotEmpty(loadParameter, property.Name, currentItemType, fillableItemType);
                ValidateDataReaderContainsRequiredField(loadParameter, property.Name, drd, fillableItemType);

                var parameterName = loadParameter.Name ?? property.Name;

                if (!CheckDataReaderContainsField(drd, parameterName))
                {
                    if (loadParameter.DefaultValue != null)
                        property.SetValue(currentItem, loadParameter.DefaultValue, null);
                    continue;
                }

                ValidateNotNullableFieldNotEqualDbNull(drd, loadParameter, property.Name, currentItemType, fillableItemType);

                var value = drd[parameterName];
                if (value == DBNull.Value)
                    continue;

                property.SetValue(currentItem, value, null);
            }
        }


        /// <summary>
        /// Проверка на поддержку автоматического чтения из SqlDataReader.
        /// </summary>
        /// <param name="loadCommand">Атрибут для автоматического чтения.</param>
        /// <param name="currentItemType">Тип заполняемого объекта.</param>
        private static void ValidateLoadCommand(LoadCommandAttribute loadCommand, Type currentItemType)
        {
            if (loadCommand == null)
                throw new MappingException(
                    string.Format("Объект '{0}' не поддерживает автоматическое чтение из SqlDataReader." +
                                  "Отсутствует LoadCommandAttribute аттрибут.", currentItemType));
            if (string.IsNullOrEmpty(loadCommand.IdFieldName))
                throw new MappingException(
                    string.Format("Объект '{0}' не поддерживает автоматическое чтение из SqlDataReader." +
                                  "Незаданы параметры аттрибута LoadCommandAttribute.", currentItemType));
        }

        /// <summary>
        /// Проверка на наличие в <see cref="SqlDataReaderWithSchema"/> обязательного поля.
        /// </summary>
        /// <param name="loadParameterAttribute">Атрибут автоматического считывания данных.</param>
        /// <param name="drd">Объект класа <see cref="SqlDataReaderWithSchema"/>.</param>
        /// <param name="fillableItemType">Тип заполняемого объекта в <see cref="SqlDataReaderWithSchema"/>.</param>
        private static void ValidateDataReaderContainsRequiredField(LoadParameterAttribute loadParameterAttribute, string parameterName, SqlDataReaderWithSchema drd, Type fillableItemType)
        {
            if (!loadParameterAttribute.Required)
                return;

            if (!CheckDataReaderContainsField(drd, loadParameterAttribute.Name ?? parameterName))
                throw new MappingException(string.Format("В SqlDataReader отсутствует обязательное поле '{0}'. Заполняется объект '{1}'.",
                        loadParameterAttribute.Name, fillableItemType));
        }

        /// <summary>
        /// Проверка на наличие в <see cref="SqlDataReaderWithSchema"/> ключевого поля
        /// </summary>
        /// <param name="drd">Объект класа <see cref="SqlDataReaderWithSchema"/>.</param>
        /// <param name="idFieldName">Название ключевого поля.</param>
        /// <param name="itemType">Тип заполняемого объекта.</param>
        /// <param name="fillableItemType">Тип корневого заполняемого объекта в <see cref="SqlDataReaderWithSchema"/>. (для рекурсии)</param>
        private static void ValidateDataReaderContainsIdField(SqlDataReaderWithSchema drd, string idFieldName, Type itemType, Type fillableItemType)
        {
            if (CheckDataReaderContainsField(drd, idFieldName))
                return;
            throw new MappingException(
                string.Format("В SqlDataReader отсутствует ключевое поле '{0}' объекта {1}. Заполняется объект '{2}'.", idFieldName, itemType, fillableItemType));
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

        /// <summary>
        /// Проверка на пустое название поля в атрибуте свойства
        /// </summary>
        /// <param name="loadParameter">Атрибут свойства</param>
        /// <param name="propertyName">Название свойства</param>
        /// <param name="currentItemType">Тип объекта, свойство которго проверяем</param>
        /// <param name="fillableItemType">Тип объекта, который заполняется</param>
        private static void ValidateFieldNameNotEmpty(
            LoadParameterAttribute loadParameter,
            string propertyName,
            Type currentItemType,
            Type fillableItemType)
        {
            if (string.IsNullOrEmpty(loadParameter.Name))
                throw new MappingException(
                    string.Format("Свойство '{0}' объекта '{1}' помечено пустым аттрибутом LoadParameter. Заполняется объект {2}.",
                        propertyName, currentItemType, fillableItemType));
        }

        /// <summary>
        /// Проверка на DbNull значение поля, помеченных как NotNull
        /// </summary>
        /// <param name="drd">Объект класа <see cref="SqlDataReaderWithSchema"/>.</param>
        /// <param name="loadParameter">Атрибут свойства</param>
        /// <param name="currentItemType">Тип объекта, свойство которго проверяем</param>
        /// <param name="fillableItemType">Тип объекта, который заполняется</param>
        private static void ValidateNotNullableFieldNotEqualDbNull(
            SqlDataReaderWithSchema drd,
            LoadParameterAttribute loadParameter,
            string parameterName,
            Type currentItemType,
            Type fillableItemType)
        {
            var name = loadParameter.Name ?? parameterName;

            if (drd[name] == DBNull.Value && !loadParameter.Nullable)
                throw new MappingException(
                    string.Format("Поле '{0}' объекта '{1}' имеет значение DBNull.Value. Заполняется объект '{2}'.",
                        name, currentItemType, fillableItemType));
        }
    }
}