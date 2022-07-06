using System;
using System.Windows.Forms;
using BetaOfficeClient.Common.Classes;
using BetaPress.BDCUtils.Common.Logging;
using BetaPress.BDCUtils.Common.Validation;

namespace Entities.ExceptionsHandlers
{
    public sealed class ExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandlingExecuter _handlingExecuter;
        private readonly ICustomLogger _logger;
        private readonly IWin32Window _handlingOwner;

        public ExceptionHandler(IWin32Window handlingOwner, ICustomLogger logger, IExceptionHandler exceptionHandler = null)
            : this(handlingOwner, logger, new ExceptionHandlingExecuter(exceptionHandler))
        {
        }

        internal ExceptionHandler(
            IWin32Window handlingOwner,
            ICustomLogger logger,
            IExceptionHandlingExecuter handlingExecuter)
        {
            ArgumentValidator.ValidateThatArgumentNotNull(handlingExecuter, "handlingExecuter");
            ArgumentValidator.ValidateThatArgumentNotNull(logger, "logger");

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
                    MessageDialog.ShowWarning(_handlingOwner, "Системная ошибка. Обратитесь в службу поддержки.");
                });
        }
    }
}