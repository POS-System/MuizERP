using System.Collections.Generic;
using System.Data.SqlClient;

namespace DataAccessLayer.DataReaders
{
    /// <summary>
    /// Формирует схему данных и предоставляет возможность их чтения из базы данных.
    /// </summary>
    internal sealed class SqlDataReaderWithSchema
    {
        private readonly Dictionary<string, int> _fields;
 
        /// <summary>
        /// Создает экземпляр класса <see cref="SqlDataReaderWithSchema"/>.
        /// </summary>
        /// <param name="drd">Экземпляр класса <see cref="SqlDataReader"/></param>.
        public SqlDataReaderWithSchema(SqlDataReader drd)
        {
            _fields = new Dictionary<string, int>();

            DataReader = drd;

            for (var i = 0; i < drd.FieldCount; i++)
            {
                _fields[drd.GetName(i).ToLower()] = i;
            }
        }

        /// <summary>
        /// Возвращает значение ячейки из объекта <see cref="SqlDataReader"/>
        /// </summary>
        /// <param name="index">Индекс столбца</param>
        public object this[int index]
        {
            get { return DataReader[index]; }
        }

        /// <summary>
        /// Возвращает значение ячейки из объекта <see cref="SqlDataReader"/>
        /// </summary>
        /// <param name="index">Название поля (столбца)</param>
        public object this[string index]
        {
            get { return DataReader[index]; }
        }

        /// <summary>
        /// Определяет, содержит ли схема поле с названием переданным в параметре
        /// </summary>
        /// <param name="fieldName">Название поля</param>
        public bool ContainsField(string fieldName)
        {
            return _fields.ContainsKey(fieldName);
        }

        /// <summary>
        /// Объект <see cref="SqlDataReader"/> с текущей схемой данных
        /// </summary>
        public SqlDataReader DataReader { get; private set; }
    }
}
