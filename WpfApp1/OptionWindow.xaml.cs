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
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        MainWindow mainWindow;
        Flat flat;
        Flats obj;

        public OptionWindow(MainWindow _mainWindow, Flat _flat)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            flat = _flat;
            titleTxtBox.Text = flat.title;
            sizeTxtBox.Text = flat.size.ToString();
            floorTxtBox.Text = flat.floor.ToString();
            roomsTxtBox.Text = flat.rooms.ToString();
            priceTxtBox.Text = flat.price.ToString();
            using (EstateAgencyEntities db = new EstateAgencyEntities())
            {
                obj = db.Flats.FirstOrDefault(f => f.Id == flat.id);
                img.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath(obj.Image)));
                if (obj.Available == true)
                {
                    leftBtn.Content = "Book";
                    rightBtn.Content = "Buy";
                }
                else if (obj.Booked == true)
                {
                    leftBtn.Content = "Cancel booking";
                    rightBtn.Content = "Buy";
                }
                else if (obj.Sold == true)
                {
                    leftBtn.Content = "Cancel";
                    rightBtn.Content = "See more";
                }
            }
        }

        private void leftBtn_Click(object sender, RoutedEventArgs e)
        {
            //закрытие окна
            if (leftBtn.Content.ToString() == "Cancel"){
                this.Close();
            }
            //отмена резервации
            else if(leftBtn.Content.ToString() == "Cancel booking")
            {
                using (EstateAgencyEntities db = new EstateAgencyEntities())
                {
                    db.Flats.FirstOrDefault(f => f.Id == obj.Id).Booked = false;
                    db.Flats.FirstOrDefault(f => f.Id == obj.Id).Available = true;
                    db.Bookings.Remove(db.Bookings.FirstOrDefault(b => b.FlatId == obj.Id));
                    db.SaveChanges();
                    this.Close();
                }
            }
            //резервация
            else if (leftBtn.Content.ToString() == "Book")
            {
                OrderingWindow orderingWindow = new OrderingWindow(obj, this, true);
                orderingWindow.ShowDialog();
            }
        }

        //при покупке
        private void rightBtn_Click(object sender, RoutedEventArgs e)
        {
            OrderingWindow orderingWindow = new OrderingWindow(obj, this);
            orderingWindow.ShowDialog();
        }
    }
}
