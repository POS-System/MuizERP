using System;
using System.Windows.Forms;
using BetaOfficeClient.Common.Classes;
using BetaOfficeServerObject.Exceptions.LogicExceptions;
using BetaPress.BDCUtils.Common.Validation;

namespace Entities.ExceptionsHandlers
{
    public sealed class LogicExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandlingExecuter _handlingExecuter;
        private readonly IWin32Window _handlingOwner;

        public LogicExceptionHandler(IWin32Window handlingOwner, IExceptionHandler exceptionHandler = null)
            : this(handlingOwner, new ExceptionHandlingExecuter(exceptionHandler))
        {
        }

        internal LogicExceptionHandler(
            IWin32Window handlingOwner, 
            IExceptionHandlingExecuter handlingExecuter)
        {
            ArgumentValidator.ValidateThatArgumentNotNull(handlingExecuter, "handlingExecuter");

            _handlingExecuter = handlingExecuter;
            _handlingOwner = handlingOwner;
        }

        public void Handle(Action action)
        {
            _handlingExecuter.ExecuteWithHandling<LogicException>(
                action,
                exception => MessageDialog.ShowWarning(_handlingOwner, exception.Message));
        }
    }
}
