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

        /// <summary>
        /// Задается выравнивание иконки,
        /// кроме того заменяется отступ между иконкой и текстом (это надо учесть при изменении Margin у imgBlock)
        /// </summary>
        public Aligns ImgAlign
        {
            set
            {
                var oldDock = DockPanel.GetDock(imgBlock);
                var newDock = (Dock)value;

                if (oldDock != newDock)
                {
                    DockPanel.SetDock(imgBlock, (Dock)value);

                    imgBlock.Margin = new Thickness(imgBlock.Margin.Right, imgBlock.Margin.Top, imgBlock.Margin.Bottom, imgBlock.Margin.Left);
                }
            }
        }

        #endregion
    }
}
