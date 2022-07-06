using Entities.Exceptions.InnerApplicationExceptions;
using System;


namespace Entities.ExceptionsHandlers
{
    public sealed class SqlSeverIsBusyExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandlingExecuter _handlingExecuter;
        private readonly ICustomLogger _logger;
        private readonly IWin32Window _handlingOwner;

        public SqlSeverIsBusyExceptionHandler(IWin32Window handlingOwner, ICustomLogger logger, IExceptionHandler exceptionHandler = null)
            : this(handlingOwner, logger, new ExceptionHandlingExecuter(exceptionHandler))
        {
        }

        internal SqlSeverIsBusyExceptionHandler(
            IWin32Window handlingOwner,
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
                    MessageDialog.ShowWarning(_handlingOwner,
                        "В настоящее время сервер занят и не может выполнить операцию.\n" +
                        "Попробуйте выполнить операцию позже или обратитесь в службу поддержки.");
                });
        }
    }
}