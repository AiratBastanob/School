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
    /// Логика взаимодействия для Zapis.xaml
    /// </summary>
    public partial class Zapis : Window
    {
        ClientService _currentRecord = new ClientService();
        SchoolEntities dbmodel = new SchoolEntities();
        private Service service = new Service();
        public Zapis(Service Getservice)
        {
            InitializeComponent();
            service = Getservice;
            NameTextBox.Text = service.Name;
            DurationTextBox.Text = service.Duration;    
            LoadCombpBox();
        }

        private void LoadCombpBox()
        {
            using (var db = new SchoolEntities())
            {
                var Clients = db.Client.ToArray();             
                foreach (var name in Clients)
                {
                    lientServiceTextBox.Items.Add(name);
                }
                lientServiceTextBox.SelectedIndex = 0;              
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (lientServiceTextBox.SelectedIndex == -1)
                errors.Append("Выберите клиента" + "\n");
            

            _currentRecord.ClientID = lientServiceTextBox.SelectedIndex + 1;
            _currentRecord.ClientID = lientServiceTextBox.SelectedIndex + 1;
            _currentRecord.TimeStart = Convert.ToDateTime(timer.Text);
            if (_currentRecord.TimeStart != null && _currentRecord.ServiceID != 0 && _currentRecord.ClientID != null)
            {
                dbmodel.ClientService.Add(_currentRecord);
            }

            dbmodel.SaveChanges();
            MessageBox.Show("Информация успешно добавлена!", "Окно оповещений");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
