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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        MainWindow mainWindow;

        public LogIn(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            //добавление в comboBox существующих пользователей
            using (EstateAgencyEntities db = new EstateAgencyEntities())
            {
                var list = (from u in db.Users
                            select u.Rights).ToList();
                comboBox.ItemsSource = list;
                comboBox.SelectedItem = comboBox.Items[1];
            }
        }

        //при нажатии на Далее
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (EstateAgencyEntities db = new EstateAgencyEntities())
            {
                //проверка совпадение пароля
                if (passwordTxt.Password == db.Users.Where(u => u.Rights == comboBox.SelectedValue.ToString()).FirstOrDefault().Password)
                {
                    mainWindow.variant = 1;
                    mainWindow.user = comboBox.SelectedValue.ToString();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Password isn't correct");
                }
            }
        }
    }
}
