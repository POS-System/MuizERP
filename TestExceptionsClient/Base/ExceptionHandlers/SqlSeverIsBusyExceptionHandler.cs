using Entities.Base.Utils;
using Entities.Base.Utils.Interface;
using Entities.Base.Utils.Loggers;
using Entities.Exceptions.InnerApplicationExceptions;
using System;

namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public sealed class SqlSeverIsBusyExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandlingExecuter _handlingExecuter;
        private readonly ICustomLogger _logger;
        private readonly IntPtr _handlingOwner;

        public SqlSeverIsBusyExceptionHandler(IntPtr handlingOwner, ICustomLogger logger, IExceptionHandler exceptionHandler = null)
            : this(handlingOwner, logger, new ExceptionHandlingExecuter(exceptionHandler))
        {
        }

        internal SqlSeverIsBusyExceptionHandler(
            IntPtr handlingOwner,
            ICustomLogger logger,
            IExceptionHandlingExecuter handlingExecuter)
        {
            //ArgumentValidator.ValidateThatArgumentNotNull(handlingExecuter, "handlingExecuter");
            //ArgumentValidator.ValidateThatArgumentNotNull(logger, "logger");

            _handlingExecuter = handlingExecuter;
            _logger = logger;
            _handlingOwner = handlingOwner;
        }

        public void Handle(Action action)
        {
            _handlingExecuter.ExecuteWithHandling<SqlSeverIsBusyException>(
                action,
                exception =>
                {
                    _logger.Error(exception);
                    //MessageDialog.ShowWarning(_handlingOwner,
                    //    "В настоящее время сервер занят и не может выполнить операцию.\n" +
                    //    "Попробуйте выполнить операцию позже или обратитесь в службу поддержки.");
                });
        }
    }
}