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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        MainWindow mainWindow;
        bool isNew;
        Flats obj;
        Flat flat;

        public EditWindow(MainWindow _mainWindow, bool _new)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            isNew = _new;
            //заполнение полей данными в случае открытия существующего объекта
            if (!isNew)
            {
                cancelBtn.Content = "Delete";
                using (EstateAgencyEntities db = new EstateAgencyEntities())
                {
                    flat = (mainWindow.flatList.SelectedItem as Flat);
                    obj = db.Flats.FirstOrDefault(f => f.Id == flat.id);
                    titleTxtBox.Text = obj.Title;
                    numberTxtBox.Text = obj.Number.ToString();
                    imageTxtBox.Text = obj.Image;
                    sizeTxtBox.Text = obj.Size.ToString();
                    floorTxtBox.Text = obj.Floor.ToString();
                    roomsTxtBox.Text = obj.Rooms.ToString();
                    priceTxtBox.Text = obj.Price.ToString();
                }
            }
        }
        
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            //при удалении существующего объекта
            if (cancelBtn.Content.ToString() == "Delete")
            {
                using (EstateAgencyEntities db = new EstateAgencyEntities())
                {
                    try
                    {
                        if (obj.Booked == true | obj.Sold == true)
                            db.Bookings.Remove(db.Bookings.FirstOrDefault(b => b.FlatId == flat.id));
                        db.Flats.Remove(db.Flats.FirstOrDefault(f => f.Id == flat.id));
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            //либо при закрытии
            this.Close();
        }

        //при сохранении изменений
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(titleTxtBox.Text) & !String.IsNullOrEmpty(imageTxtBox.Text)
                    & !String.IsNullOrEmpty(numberTxtBox.Text) & !String.IsNullOrEmpty(floorTxtBox.Text)
                    & !String.IsNullOrEmpty(roomsTxtBox.Text) & !String.IsNullOrEmpty(sizeTxtBox.Text)
                    & !String.IsNullOrEmpty(priceTxtBox.Text))
            {
                //при создании нового объекта
                if (isNew)
                {
                    try
                    {
                        BitmapImage img = new BitmapImage(new Uri(System.IO.Path.GetFullPath(imageTxtBox.Text)));
                        using (EstateAgencyEntities db = new EstateAgencyEntities())
                        {
                            db.Flats.Add(new Flats
                            {
                                Title = titleTxtBox.Text,
                                Image = imageTxtBox.Text,
                                Number = Convert.ToInt32(numberTxtBox.Text),
                                Floor = Convert.ToInt32(floorTxtBox.Text),
                                Rooms = Convert.ToInt32(roomsTxtBox.Text),
                                Size = Convert.ToInt32(sizeTxtBox.Text),
                                Price = Convert.ToInt32(priceTxtBox.Text),
                                Available = true,
                                Booked = false,
                                Sold = false
                            });
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                //при изменении существующего объекта
                else if (!isNew)
                {
                    try
                    {
                        BitmapImage img = new BitmapImage(new Uri(System.IO.Path.GetFullPath(imageTxtBox.Text)));
                        using (EstateAgencyEntities db = new EstateAgencyEntities())
                        {
                            db.Flats.FirstOrDefault(f => f.Id == obj.Id).Title = titleTxtBox.Text;
                            db.Flats.FirstOrDefault(f => f.Id == obj.Id).Image = imageTxtBox.Text;
                            db.Flats.FirstOrDefault(f => f.Id == obj.Id).Number = Convert.ToInt32(numberTxtBox.Text);
                            db.Flats.FirstOrDefault(f => f.Id == obj.Id).Floor = Convert.ToInt32(floorTxtBox.Text);
                            db.Flats.FirstOrDefault(f => f.Id == obj.Id).Rooms = Convert.ToInt32(roomsTxtBox.Text);
                            db.Flats.FirstOrDefault(f => f.Id == obj.Id).Size = Convert.ToInt32(sizeTxtBox.Text);
                            db.Flats.FirstOrDefault(f => f.Id == obj.Id).Price = Convert.ToInt32(priceTxtBox.Text);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
