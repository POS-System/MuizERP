using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MuizClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для ToolBarButtonControl.xaml
    /// </summary>
    public partial class ToolBarButtonControl : UserControl
    {
        public ToolBarButtonControl()
        {
            InitializeComponent();
        }



        #region DP

        // ImgSource
        public static readonly DependencyProperty ImgSourceProperty = DependencyProperty.Register(
                        nameof(ImgSource),
                        typeof(ImageSource),
                        typeof(ToolBarButtonControl));

        public ImageSource ImgSource
        {
            get { return (ImageSource)GetValue(ImgSourceProperty); }
            set { SetValue(ImgSourceProperty, value); }
        }

        // VisibilityButton
        public static readonly DependencyProperty VisibilityButtonProperty = DependencyProperty.Register(
                        nameof(VisibilityButton),
                        typeof(Visibility),
                        typeof(ToolBarButtonControl), new UIPropertyMetadata(Visibility.Visible));

        public Visibility VisibilityButton
        {
            get { return (Visibility)GetValue(VisibilityButtonProperty); }
            set { SetValue(VisibilityButtonProperty, value); }
        }


        // IsEnabledRule
        public static readonly DependencyProperty IsEnabledRuleProperty = DependencyProperty.Register(
                        nameof(IsEnabledRule),
                        typeof(Boolean),
                        typeof(ToolBarButtonControl), new UIPropertyMetadata(true));

        public Boolean IsEnabledRule
        {
            get { return (Boolean)GetValue(IsEnabledRuleProperty); }
            set { SetValue(IsEnabledRuleProperty, value); }
        }

        #endregion


        #region Actions

        public event Action ButtonClick;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick?.Invoke();
        }

        #endregion


        #region Other

        public enum Aligns { Left = Dock.Left, Right = Dock.Right }

        public string Text
        {
            set => textBlock.Text = value;
        }

        public Aligns ImgAlign
        {
            set => DockPanel.SetDock(imgBlock, (Dock) value);
        }

        #endregion

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    var yo = DockPanel.GetDock(imgBlock);

        //    ImgAlign = yo == Dock.Left
        //        ? Aligns.Right
        //        : Aligns.Left;
        //}
    }
}
