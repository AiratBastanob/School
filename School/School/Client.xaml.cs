using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace School
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        SchoolEntities dbmodel = new SchoolEntities();
        Service _currentService = new Service();
        private int selectprice, selectDiscount;
        List<Service> Services = new List<Service>();
        private string FilePath { get; set; }
        private byte[] imageData;
        public Client()
        {
            InitializeComponent();
            DataContext = _currentService;
            DataGridService.ItemsSource = dbmodel.Service.ToList();
            DataContext = _currentService;
            LoadComponent(false);
            LoadComboBox1();
            LoadComboBox2();
        }
        private void DataGridService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Service item = DataGridService.SelectedItem as Service;
            if (item != null)
            {
                foreach (Service service in dbmodel.Service)
                {
                    if (service.ID == item.ID)
                    {
                        _currentService = service;
                        break;
                    }
                }
            }

            UpdateButton.IsEnabled = true;
            NameTextBox.Text = _currentService.Name;
            CostServiceTextBox.Text = Convert.ToString(_currentService.Cost);
            DurationTextBox.Text = _currentService.Duration;
            MaxDiscountTextBox.Text = Convert.ToString(_currentService.Discount);
            if (_currentService.MainImageService != null)
            {
                var bitmap = new BitmapImage();
                MemoryStream ms = new MemoryStream();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                ServicePhotoImage.Source = (ImageSource)bitmap;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            bool allright = false;
            Service item = DataGridService.SelectedItem as Service;
            foreach (Service service in dbmodel.Service)
            {
                if (service.ID == item.ID)
                {
                    _currentService = service;
                    break;
                }
            }
            StringBuilder errors = new StringBuilder();
            _currentService.Name = NameTextBox.Text;
            _currentService.Cost = Convert.ToInt32(CostServiceTextBox.Text);
            _currentService.Duration = DurationTextBox.Text;
            _currentService.Discount = Convert.ToInt32(MaxDiscountTextBox.Text);

            if (_currentService.Name != null && _currentService.Cost != 0 && _currentService.Duration != null && _currentService.Discount != null)
                allright = true;
            if (allright == true)
            {
                try
                {
                    dbmodel.SaveChanges();
                    MessageBox.Show("Информация успешно изменена!", "Окно оповещений");
                    DataGridService.Items.Refresh();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var serviceForDeleting = DataGridService.SelectedItems.Cast<Service>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {serviceForDeleting.Count} элементов?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    dbmodel.Service.RemoveRange(serviceForDeleting);
                    dbmodel.SaveChanges();
                    MessageBox.Show("Данные удалены!", "Окно оповещений");
                    DataGridService.ItemsSource = dbmodel.Service.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        

        private void LoadComboBox1()
        {
            DiscountComboBox.Items.Add("Скидка меньше 5");
            DiscountComboBox.Items.Add("Скидка больше 5 меньше 15");
            DiscountComboBox.Items.Add("Скидка больше 15 меньше 30");
            DiscountComboBox.Items.Add("Скидка больше 30 меньше 70");
            DiscountComboBox.Items.Add("Скидка больше 70 меньше 100");
        }

        private void LoadComboBox2()
        {
            PriceComboBox.Items.Add("По возрастанию цена");
            PriceComboBox.Items.Add("По убыванию цена");
            PriceComboBox.Items.Add("По умолчанию цена");
        }

        /// <summary>
        /// Логика подгрузки данных при выборе фильтрации
        /// </summary>
        private void LoadComponent(bool Check)
        {
            using (var db = new SchoolEntities())
            {
                var ServicesAll = db.Service.ToList();
                if (Check == false)
                {
                    Services = ServicesAll;
                    DataGridService.ItemsSource = Services;
                }
                else
                {
                    Services = ServicesAll;
                    if (!string.IsNullOrEmpty(SearchTextBox.Text))
                    {
                        var currentProductName = dbmodel.Service.ToList();
                        currentProductName = currentProductName.Where(p => p.Description.ToLower().Contains(SearchTextBox.Text.ToLower()) || p.Name.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList();
                        Services = currentProductName;
                    }
                    if (selectprice == 0)
                    {
                        Services = Services.OrderBy(items => items.Cost).ToList();
                        DataGridService.ItemsSource = Services;
                    }
                    if (selectprice == 1)
                    {
                        Services = Services.OrderByDescending(items => items.Cost).ToList();
                        DataGridService.ItemsSource = Services;
                    }
                    if (selectprice == 2)
                    {
                        DataGridService.ItemsSource = Services;
                    }
                    if (selectDiscount == 0)
                    {
                        Services = (from items in Services where items.Discount < 5 select items).ToList();
                        DataGridService.ItemsSource = Services;
                    }

                    if (selectDiscount == 1)
                    {
                        Services = (from items in Services where items.Discount >= 15 && items.Discount < 30 select items).ToList();
                        DataGridService.ItemsSource = Services;
                    }
                    if (selectDiscount == 2)
                    {
                        Services = (from items in Services where items.Discount >= 30 && items.Discount < 70 select items).ToList();
                        DataGridService.ItemsSource = Services;
                    }
                    if (selectDiscount == 3)
                    {
                        Services = (from items in Services where items.Discount >= 70 && items.Discount < 100 select items).ToList();
                        DataGridService.ItemsSource = Services;
                    }
                }
            }
        }

        private void DiscountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int select = DiscountComboBox.SelectedIndex;
            switch (select)
            {
                case 0:
                    selectDiscount = 0;
                    break;
                case 1:
                    selectDiscount = 1;
                    break;
                case 2:
                    selectDiscount = 2;
                    break;
                case 3:
                    selectDiscount = 3;
                    break;
            }
            LoadComponent(true);
        }
        

        private void PriceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int select = PriceComboBox.SelectedIndex;
            switch (select)
            {
                case 0:
                    selectprice = 0;
                    break;
                case 1:
                    selectprice = 1;
                    break;
                case 2:
                    selectprice = 2;
                    break;
                default:
                    selectDiscount = 2;
                    break;
            }
            LoadComponent(true);
        }
    }
}
    

