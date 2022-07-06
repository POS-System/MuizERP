using System;

namespace Entities.ExceptionsHandlers
{
    public interface IExceptionHandlingExecuter
    {
        void ExecuteWithHandling<T>(Action action, Action<T> exceptionHandlingAction) where T : Exception;
    }
}
