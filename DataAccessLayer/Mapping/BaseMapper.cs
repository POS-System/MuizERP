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
    internal sealed class BaseMapper : IMapper<SqlDataReaderWithSchema, object>
    {
        public void Map(SqlDataReaderWithSchema drd, object currentItem)
        {
            var currentItemType = currentItem.GetType();

            //Получаем значения остальных параметров
            foreach (var property in currentItemType.GetProperties())
            {
                LoadParameterAttribute loadParameter = property.GetCustomAttribute<LoadParameterAttribute>();

                // если свойство не нуждается в загрузке значения, пропускаем его
                if (loadParameter == null)
                    continue;

                var parameterName = loadParameter.Name ?? property.Name;

                var value = drd[parameterName];
                if (value == DBNull.Value)
                    continue;

                property.SetValue(currentItem, value, null);
            }
        }        
    }
}