using System;
using System.Windows;

namespace MuizClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitApp();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Кнопка нажата");
        }

        private void InitApp()
        {
            ThemeChange(Themes.Light);
        }

        private void ThemeChange(Themes theme)
        {
            // определяем путь к файлу ресурсов
            var uri = new Uri($"Config/Styles/Theme{theme}.xaml", UriKind.Relative);
            //var uri = new Uri("ThemeLight.xaml", UriKind.RelativeOrAbsolute);

            // загружаем словарь ресурсов
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.Clear();
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private enum Themes
        {
            Light,
            Dark
        }
    }
}
