using PokéDex.Models;
using PokéDex.UserPages;
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

namespace PokéDex.Views
{
    /// <summary>
    /// Interaction logic for Team.xaml
    /// </summary>
    public partial class Team : Page
    {
        TeamModel team;
        MainWindow window;
        public Team()
        {
            InitializeComponent();
            team = new TeamModel();
            window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            LogInOutHandler();
        }

        private void LogInOutHandler()
        {
                MenuItem mI1 = new MenuItem();
                mI1.Header = "_LOGOUT";
                mI1.Click += LogOut;
                MenuItem mI2 = new MenuItem();
                mI2.Header = "_HOME";
                mI2.Click += HomeView;
                MainMenu.Items.Clear();
                MainMenu.Items.Add(mI1);
                MainMenu.Items.Add(mI2);
                MenuItem mI3 = new MenuItem();
                mI3.Header = "_User: " + window.username;
                mI3.HorizontalAlignment = HorizontalAlignment.Right;
                Menus.Items.Add(mI3);
        }

        private void HomeView(object sender, RoutedEventArgs e)
        {
            Home homePage = new Home();
            this.NavigationService.Navigate(homePage);
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            window.isLoggedIn = false;
            LogInOutHandler();
            Home homePage = new Home();
            this.NavigationService.Navigate(homePage);
        }
    }
}
