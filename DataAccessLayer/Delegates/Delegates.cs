using DataAccessLayer.DataReaders;
using System;
using System.Data.SqlClient;

namespace DataAccessLayer.Delegates
{
    /// <summary>
    /// Делегат для инициализаци объека <see cref="SqlCommand"/>.
    /// </summary>
    internal delegate void InitSqlCommand(SqlCommand sqlCommand);

    /// <summary>
    /// Делегат  для чтения данных из объекта <see cref="SqlDataReaderWithSchema"/>.
    /// </summary>
    internal delegate void ReadDataReaderWithSchema(SqlDataReaderWithSchema dataReader);

    /// <summary>
    /// Делегат для обратки исключения.
    /// </summary>
    internal delegate void CatchException(Exception exception);

    /// <summary>
    /// Делегат для создания  и конфигурирования объекта <see cref="SqlCommand"/>.
    /// </summary>
    internal delegate SqlCommand PrepareSetCommand(SqlConnection connection);

    /// <summary>
    /// Делегат для дополнительного конфигурирования объекта <see cref="SqlCommand"/>.
    /// </summary>
    internal delegate void ConfigureSetCommand(SqlCommand command);

    /// <summary>
    /// Делегат для выполнения в отдельной транзакции.
    /// </summary>
    internal delegate void InsideTransaction(SqlConnection connection);

    /// <summary>
    /// Обработчик исключений.
    /// </summary>
    internal delegate void HandleException(Exception exception);
    
    /// <summary>
    /// Делегат для обработки возращаемых параметров объектом <see cref="SqlCommand"/>.
    /// </summary>
    internal delegate void GetValuesFromOutputParameters(SqlParameterCollection parameters);
}
