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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int variant = 0;
        public string user = "";
        List<Flats> list = new List<Flats>();

        public MainWindow()
        {
            InitializeComponent();
            LogIn logInWindow = new LogIn(this);
            logInWindow.ShowDialog();
            StartActions();
        }

        //сброс настройек в параметрах поиска
        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            lowestPriceTxtBox.Text = "0";
            using (EstateAgencyEntities db = new EstateAgencyEntities())
            {
                var flats = (from f in db.Flats
                             select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
                list = db.Flats.ToList();
                flatList.ItemsSource = flats;
                highestTxtBox.Text = db.Flats.Max(f => f.Price).ToString();

            }
            roomsTxtBox.Text = "";
            sizeTxtBox.Text = "";
            IsAvailableRadBtn.IsChecked = false;
            IsBookingRadBtn.IsChecked = false;
            IsSoldRadBtn.IsChecked = false;
        }

        //применение настройек в параметрах поиска
        private void applyBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(highestTxtBox.Text) | String.IsNullOrWhiteSpace(highestTxtBox.Text))
            {
                using (EstateAgencyEntities db = new EstateAgencyEntities())
                {
                    highestTxtBox.Text = db.Flats.Max(f => f.Price).ToString();
                }
            }
            if (String.IsNullOrEmpty(lowestPriceTxtBox.Text) | String.IsNullOrWhiteSpace(lowestPriceTxtBox.Text))
            {
                lowestPriceTxtBox.Text = "0";
            }
            int lowestPrice;
            int highestPrice;
            if (Int32.TryParse(lowestPriceTxtBox.Text, out lowestPrice))
            {
                if (Int32.TryParse(highestTxtBox.Text, out highestPrice))
                {
                    if (!String.IsNullOrEmpty(roomsTxtBox.Text) & !String.IsNullOrWhiteSpace(roomsTxtBox.Text))
                    {
                        int rooms;
                        if (Int32.TryParse(roomsTxtBox.Text, out rooms))
                        {
                            if (!String.IsNullOrEmpty(sizeTxtBox.Text) & !String.IsNullOrWhiteSpace(sizeTxtBox.Text))
                            {
                                int size;
                                if (Int32.TryParse(sizeTxtBox.Text, out size))
                                {
                                    using (EstateAgencyEntities db = new EstateAgencyEntities())
                                    {
                                        var flats = (from f in db.Flats
                                                     where f.Price >= lowestPrice && f.Price <= highestPrice
                                                     && f.Rooms == rooms && f.Size == size
                                                     select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
                                        list = db.Flats.Where(f => f.Price >= lowestPrice && f.Price <= highestPrice && f.Rooms == rooms && f.Size == size).ToList();
                                        flatList.ItemsSource = flats;
                                    }
                                }
                                else
                                {
                                    sizeTxtBox.Text = "";
                                }
                            }
                            else
                            {
                                using (EstateAgencyEntities db = new EstateAgencyEntities())
                                {
                                    var flats = (from f in db.Flats
                                                 where f.Price >= lowestPrice && f.Price <= highestPrice
                                                 && f.Rooms == rooms
                                                 select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
                                    list = db.Flats.Where(f => f.Price >= lowestPrice && f.Price <= highestPrice && f.Rooms == rooms).ToList();
                                    flatList.ItemsSource = flats;
                                }
                            }
                        }
                        else
                        {
                            roomsTxtBox.Text = "";
                        }
                    }
                    else if (!String.IsNullOrEmpty(sizeTxtBox.Text) & !String.IsNullOrWhiteSpace(sizeTxtBox.Text))
                    {
                        int size;
                        if (Int32.TryParse(sizeTxtBox.Text, out size))
                        {
                            using (EstateAgencyEntities db = new EstateAgencyEntities())
                            {
                                var flats = (from f in db.Flats
                                             where f.Price >= lowestPrice && f.Price <= highestPrice
                                             && f.Size == size
                                             select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
                                list = db.Flats.Where(f => f.Price >= lowestPrice && f.Price <= highestPrice && f.Size == size).ToList();
                                flatList.ItemsSource = flats;
                            }
                        }
                    }
                    else
                    {
                        using (EstateAgencyEntities db = new EstateAgencyEntities())
                        {
                            var flats = (from f in db.Flats
                                         where f.Price >= lowestPrice && f.Price <= highestPrice
                                         select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
                            list = db.Flats.Where(f => f.Price >= lowestPrice && f.Price <= highestPrice).ToList();
                            flatList.ItemsSource = flats;
                        }
                    }
                }
                else
                {
                    using (EstateAgencyEntities db = new EstateAgencyEntities())
                    {
                        highestTxtBox.Text = db.Flats.Max(f => f.Price).ToString();
                    }
                }
            }
            else
            {
                lowestPriceTxtBox.Text = "0";
            }
        }

        //при выборе свободных квартир
        private void IsAvailableRadBtn_Checked(object sender, RoutedEventArgs e)
        {
            var flats = (from f in list
                         where f.Available == true
                         select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
            flatList.ItemsSource = flats;
        }

        //при выборе зарезервированых квартир
        private void IsBookingRadBtn_Checked(object sender, RoutedEventArgs e)
        {
            using (EstateAgencyEntities db = new EstateAgencyEntities())
            {
                var flats = (from f in list
                             where f.Booked == true
                             select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
                flatList.ItemsSource = flats;
            }
        }

        //при выборе проданых квартир
        private void IsSoldRadBtn_Checked(object sender, RoutedEventArgs e)
        {
            var flats = (from f in list
                         where f.Sold == true
                         select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
            flatList.ItemsSource = flats;
        }

        //выход из учетной записи
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            variant = 0;
            grid1.Visibility = Visibility.Hidden;
            LogIn logInWindow = new LogIn(this);
            logInWindow.ShowDialog();
            if (variant != 0)
                grid1.Visibility = Visibility.Visible;
            StartActions();
            resetBtn_Click(sender, e);
        }

        //отображение\скрытие статистики проданых квартир
        private void statisticBtn_Click(object sender, RoutedEventArgs e)
        {
            if (soldFlatsList.Visibility == Visibility.Visible)
                soldFlatsList.Visibility = Visibility.Hidden;
            else
            {
                soldFlatsList.Visibility = Visibility.Visible;
                using (EstateAgencyEntities db = new EstateAgencyEntities())
                {
                    var stat = (from s in db.Flats.Where(f => f.Sold == true)
                                group s by s.Rooms into flat
                                select new
                                {
                                    Rooms = flat.Key,
                                    Price = flat.Sum(s => s.Price)
                                }).OrderByDescending(o => o.Rooms).ToList();
                    soldFlatsList.ItemsSource = stat;
                }
            }
        }

        //при двойном нажатии на объект listView
        //открывает окно редактирования или действий в зависимости от прав
        private void flatList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (user == "Estate agent")
            {
                OptionWindow optionWindow = new OptionWindow(this, flatList.SelectedItem as Flat);
                optionWindow.ShowDialog();
            }
            else if (user == "Admin")
            {
                EditWindow editWindow = new EditWindow(this, false);
                editWindow.ShowDialog();
            }
            UpdateData();
        }

        //только при правах админа - создание новой квартиры
        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow = new EditWindow(this, true);
            editWindow.ShowDialog();
            using (EstateAgencyEntities db = new EstateAgencyEntities())
            {
                var flats = (from f in db.Flats
                             select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
                list = db.Flats.ToList();
                flatList.ItemsSource = flats;
            }
        }

        //заполнение окна данными
        private void StartActions()
        {
            //если не был введен пароль и закрыта окно входа нечего не отобразится
            if (variant == 0)
            {
                grid1.Visibility = Visibility.Hidden;
            }

            //заполнение listView
            using (EstateAgencyEntities db = new EstateAgencyEntities())
            {
                var flats = (from f in db.Flats
                             select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
                list = db.Flats.ToList();
                flatList.ItemsSource = flats;
                highestTxtBox.Text = db.Flats.Max(f => f.Price).ToString();
            }
            lowestPriceTxtBox.Text = "0";
            userName.Text = user;

            //отображение\скрытие кнопки добавления новой квартиры
            if (user == "Admin")
                newBtn.Visibility = Visibility.Visible;
            else if (user == "Estate agent")
                newBtn.Visibility = Visibility.Hidden;
        }

        //обновление данных в listView & Statistic
        private void UpdateData()
        {
            using (EstateAgencyEntities db = new EstateAgencyEntities())
            {
                var flats = (from f in db.Flats
                             select new Flat { id = f.Id, title = f.Title, number = f.Number, price = f.Price, size = f.Size, rooms = f.Rooms, floor = f.Floor }).ToList();
                list = db.Flats.ToList();
                flatList.ItemsSource = flats;
                var stat = (from s in db.Flats.Where(f => f.Sold == true)
                            group s by s.Rooms into flat
                            select new
                            {
                                Rooms = flat.Key,
                                Price = flat.Sum(s => s.Price)
                            }).OrderByDescending(o => o.Rooms).ToList();
                soldFlatsList.ItemsSource = stat;
            }

        }
    }
}
