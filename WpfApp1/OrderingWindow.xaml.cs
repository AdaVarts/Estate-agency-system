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
    /// Interaction logic for OrderingWindow.xaml
    /// </summary>
    public partial class OrderingWindow : Window
    {
        Flats obj;
        OptionWindow window;
        public bool book;

        public OrderingWindow(Flats _obj, OptionWindow _window, bool _book = false)
        {
            InitializeComponent();
            obj = _obj;
            window = _window;
            book = _book;
            tittleTxtBox.Text = obj.Title;
            priceTxtBox.Text = obj.Price.ToString();
            if (obj.Available == true)
            {
                if (book)
                    BookBuyBtn.Content = "Book";
                else
                    BookBuyBtn.Content = "Buy";
            }
            else 
            {
                using (EstateAgencyEntities db = new EstateAgencyEntities()) 
                {
                    var res = db.Bookings.FirstOrDefault(b => b.FlatId == obj.Id);
                    fNameTxtBox.Text = res.FirstName;
                    lNameTxtBox.Text = res.LastName;
                    fNameTxtBox.Focusable = false;
                    lNameTxtBox.Focusable = false;
                }
                if (obj.Sold == true)
                    BookBuyBtn.Visibility = Visibility.Hidden;
                else if (obj.Booked == true)
                    BookBuyBtn.Content = "Buy";
            }
        }

        //закрытие окна
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BookBuyBtn_Click(object sender, RoutedEventArgs e)
        {
            //покупка в случае предварительной резервации
            if (BookBuyBtn.Content.ToString() == "Buy" & obj.Booked==true)
            {
                using (EstateAgencyEntities db = new EstateAgencyEntities())
                {
                    db.Bookings.FirstOrDefault(b => b.FlatId == obj.Id).Sold = true;
                    db.Flats.FirstOrDefault(f => f.Id == obj.Id).Booked = false;
                    db.Flats.FirstOrDefault(f => f.Id == obj.Id).Sold = true;
                    db.SaveChanges();
                    this.Close();
                    window.Close();
                }
            }
            //покупка без предварительной резервации
            else if (BookBuyBtn.Content.ToString() == "Buy")
            {
                if (!String.IsNullOrEmpty(fNameTxtBox.Text) & !String.IsNullOrEmpty(lNameTxtBox.Text))
                {
                    using (EstateAgencyEntities db = new EstateAgencyEntities())
                    {
                        db.Bookings.Add(new Bookings { FirstName = fNameTxtBox.Text, LastName = lNameTxtBox.Text, FlatId = obj.Id, Sold = true });
                        db.Flats.FirstOrDefault(f => f.Id == obj.Id).Available = false;
                        db.Flats.FirstOrDefault(f => f.Id == obj.Id).Sold = true;
                        db.SaveChanges();
                        this.Close();
                        window.Close();
                    }
                }
            }
            //резервация
            else if (BookBuyBtn.Content.ToString() == "Book")
            {
                if (!String.IsNullOrEmpty(fNameTxtBox.Text) & !String.IsNullOrEmpty(lNameTxtBox.Text))
                {
                    using (EstateAgencyEntities db = new EstateAgencyEntities())
                    {
                        db.Bookings.Add(new Bookings { FirstName = fNameTxtBox.Text, LastName = lNameTxtBox.Text, FlatId = obj.Id, Sold = false });
                        db.Flats.FirstOrDefault(f => f.Id == obj.Id).Available = false;
                        db.Flats.FirstOrDefault(f => f.Id == obj.Id).Booked = true;
                        db.SaveChanges();
                        this.Close();
                        window.Close();
                    }
                }
            }
        }
    }
}
