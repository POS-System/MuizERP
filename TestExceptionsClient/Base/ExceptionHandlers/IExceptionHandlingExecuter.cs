using System;

namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public interface IExceptionHandlingExecuter
    {
        void ExecuteWithHandling<T>(Action action, Action<T> exceptionHandlingAction) where T : Exception;
    }
}
