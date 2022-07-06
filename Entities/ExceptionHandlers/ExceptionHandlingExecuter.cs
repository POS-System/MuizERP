using System;
using BetaPress.BDCUtils.Common.Validation;

namespace Entities.ExceptionsHandlers
{
    public sealed class ExceptionHandlingExecuter : IExceptionHandlingExecuter
    {
        private readonly IExceptionHandler _innerExceptionHandler;

        public ExceptionHandlingExecuter(
            IExceptionHandler innerExceptionHandler = null)
        {
            _innerExceptionHandler = innerExceptionHandler;
        }

        public void ExecuteWithHandling<T>(Action action, Action<T> exceptionHandlingAction) where T : Exception
        {
            ArgumentValidator.ValidateThatArgumentNotNull(action, "action");
            ArgumentValidator.ValidateThatArgumentNotNull(exceptionHandlingAction, "exceptionHandlingAction");

            try
            {
                if (_innerExceptionHandler != null)
                {
                    _innerExceptionHandler.Handle(action);
                }
                else
                {
                    action();
                }
            }
            catch (Exception exception)
            {
                if (exception is T)
                    exceptionHandlingAction(exception as T);
                else
                    throw;
            }
        }
    }
}