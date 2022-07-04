using System;
using System.Windows;
using System.Windows.Controls;

namespace MuizClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для ToolBarControl.xaml
    /// </summary>
    public partial class ToolBarControl : UserControl
    {
        public ToolBarControl()
        {
            InitializeComponent();
        }


        #region DP

        // EditButtonEnabled
        public static readonly DependencyProperty EditButtonEnabledProperty = DependencyProperty.Register(
                        nameof(EditButtonEnabled),
                        typeof(bool),
                        typeof(ToolBarControl));

        public bool EditButtonEnabled
        {
            get { return (bool)GetValue(EditButtonEnabledProperty); }
            set { SetValue(EditButtonEnabledProperty, value); }
        }

        // RemoveButtonEnabled
        public static readonly DependencyProperty RemoveButtonEnabledProperty = DependencyProperty.Register(
                        nameof(RemoveButtonEnabled),
                        typeof(bool),
                        typeof(ToolBarControl));

        public bool RemoveButtonEnabled
        {
            get { return (bool)GetValue(RemoveButtonEnabledProperty); }
            set { SetValue(RemoveButtonEnabledProperty, value); }
        }

        #endregion


        #region Actions

        public event Action AddButtonClick;
        public event Action EditButtonClick;
        public event Action RemoveButtonClick;
        public event Action RefreshButtonClick;

        private void Add_Button_Click() => AddButtonClick?.Invoke();

        private void Edit_Button_Click() => EditButtonClick?.Invoke();

        private void Remove_Button_Click() => RemoveButtonClick?.Invoke();

        private void Refresh_Button_Click() => RefreshButtonClick?.Invoke();

        #endregion
    }
}
