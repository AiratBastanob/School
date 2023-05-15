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
        public Zapis()
        {
            InitializeComponent();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentRecord.TimeStart != null && _currentRecord.ServiceID != 0 && _currentRecord.ClientID != null)
            {
                dbmodel.ClientService.Add(_currentRecord);
            }

            dbmodel.SaveChanges();
            MessageBox.Show("Информация успешно добавлена!", "Окно оповещений");
        }
    }
}
