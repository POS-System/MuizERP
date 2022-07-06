using System;

namespace Entities.ExceptionsHandlers
{
    public interface IExceptionHandler
    {
        void Handle(Action action);
    }
}
