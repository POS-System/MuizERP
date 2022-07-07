using System;

namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public interface IExceptionHandler
    {
        void Handle(Action action);
    }
}
