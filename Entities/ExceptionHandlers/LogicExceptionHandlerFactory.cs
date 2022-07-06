using System;
using System.Windows.Forms;
using BetaPress.BDCUtils.Common.Factories;
using BetaPress.BDCUtils.Common.Logging;

namespace Entities.ExceptionsHandlers
{
    public sealed class LogicExceptionHandlerFactory : 
        IKeyedFactory<IExceptionHandler, Tuple<IWin32Window, ICustomLogger>>
    {
        public IExceptionHandler Create(Tuple<IWin32Window, ICustomLogger> param)
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
