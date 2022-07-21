using Entities.Base.Utils.Interface;
using System;

namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public sealed class SystemExceptionHandlerFactory :
        IKeyedFactory<Tuple<IntPtr, ICustomLogger>, IExceptionHandler>
    {
        public IExceptionHandler Create(Tuple<IntPtr, ICustomLogger> param)
        {
            return new ExceptionHandler(param.Item1, param.Item2);
        }
    }
}