using System;
using Entities.Base.Utils;

namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public sealed class ExceptionHandlerFactory : 
        IKeyedFactory<IExceptionHandler, Tuple<IntPtr, ICustomLogger>>
    {
        public IExceptionHandler Create(Tuple<IntPtr, ICustomLogger> param)
        {
            return new ExceptionHandler(
                    param.Item1,
                    param.Item2,
                    new LogicExceptionHandler(
                        param.Item1,
                        new SqlSeverIsBusyExceptionHandler(
                            param.Item1,
                            param.Item2,
                            new OperationCanceledExceptionHandler())));
        }
    }
}
