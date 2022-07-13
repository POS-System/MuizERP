using MuizClient.Helpers.FilterValue;

namespace MuizClient.Controls.Grid.GridFilter.GridFilterControls
{
    /// <summary>
    /// Логика взаимодействия для IntFilterControl.xaml
    /// </summary>
    public partial class IntFilterControl : BaseFilterControl<IntFilterValue, IntFilterControl>
    {
        public IntFilterControl()
        {
            CurrentValue.FromValue = MinValue;
            CurrentValue.ToValue = MaxValue;

            InitializeComponent();
        }


        public int MinValue { get; } = 0;
        public int MaxValue { get; } = 1000000;


        //public static readonly DependencyProperty LowerValueProperty =
        //    DependencyProperty.Register(nameof(Lower), typeof(IntFilterValue), typeof(IntFilterControl));
        //public static readonly DependencyProperty UpperValueProperty =
        //    DependencyProperty.Register(nameof(Upper), typeof(IntFilterValue), typeof(IntFilterControl));


        //public IntFilterValue Lower
        //{
        //    get => (IntFilterValue)GetValue(LowerValueProperty);
        //    set 
        //    { 
        //        SetValue(LowerValueProperty, value); 
        //        OnPropertyChanged(); 
        //    }
        //}

        //public IntFilterValue Upper
        //{
        //    get => (IntFilterValue)GetValue(UpperValueProperty);
        //    set 
        //    { 
        //        SetValue(UpperValueProperty, value); 
        //        OnPropertyChanged(); 
        //    }
        //}
    }
}
