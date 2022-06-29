using MuizClient.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace MuizClient.Controls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl, INotifyPropertyChanged
    {
        ObservableCollection<Phone> phonesList;
        CollectionViewSource collectionVS;

        public UserControl1()
        {
            InitializeComponent();

            phonesList = new ObservableCollection<Phone>();

            Phone phone = new Phone();
            phone.Title = "iPhone" + (phonesList.Count + 1).ToString();
            phone.Company = "Company" + (phonesList.Count + 1).ToString();
            phone.Price = (phonesList.Count + 1) * 100;
            phonesList.Add(phone);
            
            //phonesGrid.ItemsSource = phonesList;

            collectionVS = new CollectionViewSource();
            collectionVS.Source = phonesList;
            collectionVS.Filter += new FilterEventHandler(CollectionViewSource_Filter);
            collectionVS.IsLiveFilteringRequested = true;

            phonesGrid.IsSynchronizedWithCurrentItem = true;
            phonesGrid.ItemsSource = collectionVS.View;           
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            Phone item = e.Item as Phone;

            e.Accepted = !(item?.Price / 10 % 2 == 1);
        }

        private void Button_Click()
        {
            Phone phone = new Phone();
            phone.Title = "iPhone" + (phonesList.Count + 1).ToString();
            phone.Company = "Company" + (phonesList.Count + 1).ToString();
            phone.Price = phonesList.Count * 100;
            phonesList.Add(phone);
        }

        private void Button_Click_1()
        {
            Phone phone = phonesGrid.SelectedItem as Phone;

            if (phone != null)
                phone.Price = phone.Price + 10;
        }

        private void Button_Click_2()
        {
            Phone phone = phonesGrid.SelectedItem as Phone;

            if (phone != null)
                phonesList.Remove(phone);


            phonesGrid.UpdateLayout();
        }


        private bool _isHasSelected;

        public bool IsHasSelected 
        {
            get => _isHasSelected;

            set
            {
                _isHasSelected = value;
                OnPropertyChanged(nameof(IsHasSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click_3(object sender, System.Windows.RoutedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(phonesGrid.ItemsSource);

            collectionVS.Filter += new FilterEventHandler(CollectionViewSource_Filter);
            view.Refresh();
        }

        private void Button_Click_4(object sender, System.Windows.RoutedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(phonesGrid.ItemsSource);

            view.Filter = null;
        }

        private void phonesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Phone phone = ((DataGrid)sender).SelectedItem as Phone;

            IsHasSelected = phone != null;
        }
    }
}
