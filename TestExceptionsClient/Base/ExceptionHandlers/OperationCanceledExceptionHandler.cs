using System;

namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public sealed class OperationCanceledExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandlingExecuter _handlingExecuter;

        public OperationCanceledExceptionHandler(IExceptionHandler exceptionHandler = null)
            : this( new ExceptionHandlingExecuter(exceptionHandler))
        {
        }

        internal OperationCanceledExceptionHandler(IExceptionHandlingExecuter handlingExecuter)
        {
            //ArgumentValidator.ValidateThatArgumentNotNull(handlingExecuter, "handlingExecuter");

            _handlingExecuter = handlingExecuter;
        }

        public void Handle(Action action)
        {
            //ArgumentValidator.ValidateThatArgumentNotNull(action, "action");

            _handlingExecuter.ExecuteWithHandling<OperationCanceledException>(
                action,
                exception => {});
        }
    }
}
