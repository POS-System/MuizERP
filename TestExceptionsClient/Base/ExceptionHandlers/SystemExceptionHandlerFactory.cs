using Entities.Base.Utils;
using Entities.Base.Utils.Interface;
using System;

namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public sealed class SystemExceptionHandlerFactory :
        IKeyedFactory<IExceptionHandler, Tuple<IntPtr, ICustomLogger>>
    {
        public IExceptionHandler Create(Tuple<IntPtr, ICustomLogger> param)
        {
            return new ExceptionHandler(param.Item1, param.Item2);
        }
    }
}