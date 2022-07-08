using MuizClient.Helpers.FilterValue;
using System.Windows;

namespace MuizClient.Controls.Grid.GridFilter.GridFilterControls
{
    /// <summary>
    /// Логика взаимодействия для Double.xaml
    /// </summary>
    public partial class DoubleFilterControl : BaseFilterControl<DoubleFilterValue, DoubleFilterControl>
    {
        public DoubleFilterControl()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty LowerValueProperty =
            DependencyProperty.Register(nameof(LowerValue), typeof(double), typeof(DoubleFilterControl));
        public static readonly DependencyProperty UpperValueProperty =
            DependencyProperty.Register(nameof(UpperValue), typeof(double), typeof(DoubleFilterControl));



        //public double Minimum { get; set; }
        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }
    }
}
