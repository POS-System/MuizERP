using Entities.Base.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using TestExceptionsClient.Base.ExceptionHandlers;

namespace TestExceptionsClient.Base
{
    /// <summary>
    /// Interaction logic for BaseControl.xaml
    /// </summary>
    public partial class BaseControl : UserControl
    {
        protected IExceptionHandler LogicExceptionHandler { get; private set; }

        public BaseControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LogicExceptionHandler = new ExceptionHandlerFactory().Create(
                new Tuple<IntPtr, ICustomLogger>(
                    new WindowInteropHelper(Window.GetWindow(this)).Handle,
                    new LoggerWrapper()));

        }
    }
}
