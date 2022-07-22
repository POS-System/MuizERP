using System;
using Entities.Base.Utils.Factories;
using Entities.Base.Utils.Loggers;

namespace TestExceptionsClient.Base.ExceptionHandlers
{
    public sealed class ExceptionHandlerFactory : 
        IKeyedFactory<Tuple<IntPtr, ICustomLogger>, IExceptionHandler>
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
