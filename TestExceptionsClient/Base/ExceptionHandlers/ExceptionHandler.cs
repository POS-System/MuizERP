using Entities.Base.Utils;
using System;


namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public sealed class ExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandlingExecuter _handlingExecuter;
        private readonly ICustomLogger _logger;
        private readonly IntPtr _handlingOwner;

        public ExceptionHandler(IntPtr handlingOwner, ICustomLogger logger, IExceptionHandler exceptionHandler = null)
            : this(handlingOwner, logger, new ExceptionHandlingExecuter(exceptionHandler))
        {
        }

        internal ExceptionHandler(
            IntPtr handlingOwner,
            ICustomLogger logger,
            IExceptionHandlingExecuter handlingExecuter)
        {
            //ArgumentValidator.ValidateThatArgumentNotNull(handlingExecuter, "handlingExecuter");
            //ArgumentValidator.ValidateThatArgumentNotNull(logger, "logger");

            _handlingExecuter = handlingExecuter;
            _handlingOwner = handlingOwner;
            _logger = logger;
        }

        public void Handle(Action action)
        {
            _handlingExecuter.ExecuteWithHandling<Exception>(
                action,
                exception =>
                {
                    _logger.Error(exception);
                    //MessageDialog.ShowWarning(_handlingOwner, "Системная ошибка. Обратитесь в службу поддержки.");
                });
        }
    }
}