using System;
using System.Runtime.Serialization;

namespace Entities.Exceptions.LogicExceptions
{

    /// <summary>
    /// Абстрактный класс исключения для представления исключительных ситуаций, характерных для бизнес-процессов, моделируемых приложением
    /// </summary>
    [Serializable]
    public class LogicException : Exception
    {
        /// <summary>
        /// Конструктор для сериализации
        /// </summary>
        /// <param name="info">Информация</param>
        /// <param name="context">Контекст</param>
        public LogicException(SerializationInfo info, StreamingContext context) : base(info, context)
        {}

        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        /// <param name="message"></param>
        public LogicException(String message)
            : base(message)
        {}

    }
}
