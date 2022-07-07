using System;
using Entities.Exceptions.LogicExceptions;

namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public sealed class LogicExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandlingExecuter _handlingExecuter;
        private readonly IntPtr _handlingOwner;

        public LogicExceptionHandler(IntPtr handlingOwner, IExceptionHandler exceptionHandler = null)
            : this(handlingOwner, new ExceptionHandlingExecuter(exceptionHandler))
        {
        }

        internal LogicExceptionHandler(
            IntPtr handlingOwner, 
            IExceptionHandlingExecuter handlingExecuter)
        {
            //ArgumentValidator.ValidateThatArgumentNotNull(handlingExecuter, "handlingExecuter");

            _handlingExecuter = handlingExecuter;
            _handlingOwner = handlingOwner;
        }

        public void Handle(Action action)
        {
            _handlingExecuter.ExecuteWithHandling<LogicException>(
                action,
                exception =>
                {
                    //MessageDialog.ShowWarning(_handlingOwner, exception.Message);
                });
        }
    }
}
