using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MuizClient.Controls.Grid.GridFilter.GridFilterControls
{
    public interface IBaseFilterControl : INotifyPropertyChanged
    {
        GridColumnInfo ColumnInfo { get; set; }
        string PropertyName { get; set; }
        void Init(GridColumnInfo columnInfo);
        object CurrentValueObject { get; }
        void UpdateFilter();
        void OnPropertyChanged(string propertyName);
    }

    public class BaseFilterControl<T, V> : UserControl, IBaseFilterControl
        where V : IBaseFilterControl
    {
        //public bool IsActive { get; set; }
        public GridColumnInfo ColumnInfo { get; set; }


        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(V),
                new FrameworkPropertyMetadata() { AffectsParentArrange = true, AffectsParentMeasure = true });

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(nameof(CurrentValue), typeof(T), typeof(V),
                new PropertyMetadata(OnCurrentValueChangedCallBack));

        private static void OnCurrentValueChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as IBaseFilterControl;

            if (sender != null)
                sender.OnPropertyChanged(nameof(CurrentValue));
        }

        public T CurrentValue
        {
            get { return (T)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }

        public virtual void Init(GridColumnInfo columnInfo)
        {
            ColumnInfo = columnInfo;
            //CurrentValue = (T)ColumnInfo?.Filter?.Value ?? typeof(T).Defau.GetType().;
            PropertyName = ColumnInfo?.PropInfo?.Name; // TODO: рассмотреть возможность изменения на Title

            var filterValue = ColumnInfo?.Filter?.Value;
            if (filterValue != null) CurrentValue = (T)filterValue;
        }

        public void UpdateFilter()
        {
            var isActive = ColumnInfo.Filter.IsActive;

            if (isActive)
            {
                var curValue = (object)CurrentValue;
                var oldValue = ColumnInfo.Filter.Value;

                if (curValue != oldValue)
                {
                    ColumnInfo.Filter.Value = curValue;
                }
            }
            else
            {
                ColumnInfo.Filter.Value = null;
            }
        }

        //public virtual void Init(string propertyName)
        //{
        //    PropertyName = propertyName;
        //}

        public object CurrentValueObject { get => (object)CurrentValue; }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
