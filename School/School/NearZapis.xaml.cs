using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для NearZapis.xaml
    /// </summary>
    public partial class NearZapis : Window
    {
        SchoolEntities dbmodel = new SchoolEntities(); 
        ClientService _currentClientService = new ClientService();
        public NearZapis()
        {
            InitializeComponent();
            DataGridService.ItemsSource = dbmodel.ClientService.OrderBy(x => x.TimeStart).ToList();
        }

        private void DataGridService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientService item = DataGridService.SelectedItem as ClientService;
            if (item != null)
            {
                foreach (ClientService service in dbmodel.ClientService)
                {
                    if (service.ID == item.ID)
                    {
                        _currentClientService = service;
                        break;
                    }
                }
            }
            NameTextBox.Text = (from s in dbmodel.Service where s.ID == _currentClientService.ServiceID select s.Name).FirstOrDefault();
            SurnameClientTextBox.Text = (from c in dbmodel.Client where c.IDClient == _currentClientService.ClientID select c.Surname).FirstOrDefault();
            NameClientTextBox.Text = (from c in dbmodel.Client where c.IDClient == _currentClientService.ClientID select c.Name).FirstOrDefault();
            PatronymicClientTextBox.Text = (from c in dbmodel.Client where c.IDClient == _currentClientService.ClientID select c.Patronymic).FirstOrDefault();
            EmailClientTextBox.Text = (from c in dbmodel.Client where c.IDClient == _currentClientService.ClientID select c.Email).FirstOrDefault();
            PhoneClientTextBox.Text = (from c in dbmodel.Client where c.IDClient == _currentClientService.ClientID select c.Phone).FirstOrDefault();
            TimeTextBox.Text = _currentClientService.TimeStart.ToString();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();        
        }
    }
}
