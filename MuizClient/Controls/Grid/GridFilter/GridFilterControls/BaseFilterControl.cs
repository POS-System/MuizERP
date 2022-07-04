using System.Windows;
using System.Windows.Controls;

namespace MuizClient.Controls.Grid.GridFilter.GridFilterControls
{
    public class BaseFilterControl<T> : UserControl
    {

        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(BaseFilterControl<>));

        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        public static readonly DependencyProperty CurrentFilterValueProperty =
            DependencyProperty.Register("FilterValue", typeof(T), typeof(BaseFilterControl<>));

        public T CurrentFilterValue
        {
            get { return (T)GetValue(CurrentFilterValueProperty); }
            set { SetValue(CurrentFilterValueProperty, value); }
        }

        public virtual void Init(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
