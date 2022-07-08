using MuizClient.Helpers.FilterValue;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MuizClient.Controls.Grid.GridFilter.GridFilterControls
{
    public interface IBaseFilterControl : INotifyPropertyChanged
    {
        GridColumnInfo ColumnInfo { get; set; }
        bool IsActive { get; set; }
        string PropertyName { get; set; }
        void Init(GridColumnInfo columnInfo);
        object CurrentValueObject { get; }
        void UpdateFilter();
        void OnPropertyChanged(string propertyName);
        event Action AnyFilterChanged;
    }

    public class BaseFilterControl<T, V> : UserControl, IBaseFilterControl
        where T : IFilterValue
        where V : IBaseFilterControl
    {
        public event Action AnyFilterChanged;
        public GridColumnInfo ColumnInfo { get; set; }


        public BaseFilterControl()
        {
            CurrentValue.PropertyChanged += (object sender, PropertyChangedEventArgs e) => { UpdateFilter(); };
        }


        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(V),
                new PropertyMetadata(OnPropertyChangedCallBack));

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }


        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(V));

        //public static readonly DependencyProperty PropertyNameProperty =
        //    DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(V),
        //        new FrameworkPropertyMetadata() { AffectsParentArrange = true, AffectsParentMeasure = true });

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(nameof(CurrentValue), typeof(T), typeof(V));
        //new PropertyMetadata(OnPropertyChangedCallBack));

        private static void OnPropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as IBaseFilterControl;

            if (sender != null)
            {
                sender.UpdateFilter();
                sender.OnPropertyChanged(nameof(CurrentValue));
            }
        }

        public T CurrentValue
        {
            get 
            { 
                var result = GetValue(CurrentValueProperty);
                if (result == null)
                {
                    result = Activator.CreateInstance(typeof(T));
                    SetValue(CurrentValueProperty, result);
                }

                return (T)result;
            }
            set { SetValue(CurrentValueProperty, value); }
        }

        public virtual void Init(GridColumnInfo columnInfo)
        {
            ColumnInfo = columnInfo;
            PropertyName = ColumnInfo?.PropInfo?.Name; // TODO: рассмотреть возможность изменения на Title

            var filterValue = ColumnInfo?.Filter?.Value;
            if (filterValue != null) CurrentValue = (T)filterValue;
        }

        public void UpdateFilter()
        {
            if (ColumnInfo != null)
            {
                var oldIsActive = ColumnInfo.Filter.IsActive;
                var isActive = IsActive;

                if (isActive)
                {
                    var curValue = /*(object)*/CurrentValue as IFilterValue;
                    var oldValue = ColumnInfo.Filter.Value;

                    if (curValue != oldValue)
                    {
                        ColumnInfo.Filter.Value = curValue;
                    }
                }
                else if (oldIsActive == isActive)
                {
                    IsActive = true;
                    return;
                }

                ColumnInfo.Filter.IsActive = isActive;
                AnyFilterChanged?.Invoke();
            }
        }

        public object CurrentValueObject { get => (object)CurrentValue; }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
