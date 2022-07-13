using Entities.Base.Attributes.Interface;
using Entities.Base.Utils;
using System;

namespace Entities.Base.Attributes
{
    /// <summary>
    /// Задаёт значение параметра (свойства) <see cref="Name"/> аттрибута <see cref="TAttr"/>,
    /// установленного на свойстве с названием <see cref="propertyName"/> отбъекта <see cref="TEntity"/>
    /// </summary>
    /// <typeparam name="TAttr">Аттрибут, значение параметра (свойства) которого изменяется.</typeparam>
    /// <typeparam name="TEntity">Тип объекта, у которого изменяется аттрибут свойства.</typeparam>
    public class AttributeNameSetter<TAttr, TEntity> : IAttributeSetter<TAttr, TEntity>
        where TAttr : Attribute
        where TEntity : BaseEntity
    {
        public void Set(string propertyName, object value)
        {
            var type = typeof(TEntity);

            type.SetCustomAttributeProperty<TAttr>(propertyName, "Name", value);
        }
    }

}
