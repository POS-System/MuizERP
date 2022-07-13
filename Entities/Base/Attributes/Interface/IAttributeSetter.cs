
namespace Entities.Base.Attributes.Interface
{
    /// <summary>
    /// Интерфейс для установки параметра (свойства) аттрибута <see cref="TAttribute"/>,
    /// находящегося на одном из свойств объекта <see cref="TEntity"/>.
    /// </summary>
    /// <typeparam name="TAttribute">Тип аттрибута.</typeparam>
    /// <typeparam name="TEntity">Тип объекта.</typeparam>
    public interface IAttributeSetter<TAttribute, TEntity>
    {
        /// <summary>
        /// Устанавливает параметр (свойство) аттрибута <see cref="TAttribute"/>
        /// для свойства объекта <see cref="TEntity"/> с названием <see cref="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">Название свойства объекта <see cref="TEntity"/>, на котором располагается изменяемый аттрибут</param>
        /// <param name="value">Значение изменяемого аттрибута.</param>
        void Set(string propertyName, object value);
    }
}
