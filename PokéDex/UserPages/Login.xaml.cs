﻿using PokéDex.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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

namespace PokéDex.UserPages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        MainWindow window;
        public Login()
        {
            InitializeComponent();
            window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            LogInOutHandler();
        }

        private void LogInOutHandler()
        {
                MenuItem mI1 = new MenuItem();
                mI1.Header = "_HOME";
                mI1.Click += HomeView;
                MenuItem mI2 = new MenuItem();
                mI2.Header = "_REGISTER";
                mI2.Click += Register;
                MainMenu.Items.Clear();
                MainMenu.Items.Add(mI1);
                MainMenu.Items.Add(mI2);
        }

        private void HomeView(object sender, RoutedEventArgs e)
        {
            Home homePage = new Home();
            this.NavigationService.Navigate(homePage);
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            LogInOutHandler();
            Register registerPage = new Register();
            this.NavigationService.Navigate(registerPage);
        }

        public void DisplayError(string error)
        {
            tbxUsername.Clear();
            pbxPassword.Clear();
            MessageBox.Show(error);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbxUsername.Text) && !String.IsNullOrWhiteSpace(pbxPassword.Password))
            {
                using (var db = new PokedexTeamBuilderDBEntities())
                {
                    var query = db.Users.Where(u => u.username == tbxUsername.Text);
                    var loadUser = query.ToList();
                    if (loadUser.Count > 0)
                    {
                        var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        window.isLoggedIn = true;
                        window.username = tbxUsername.Text;
                        Views.Home home = new Views.Home();
                        NavigationService.Navigate(home);
                    }
                    else
                    {
                        DisplayError("The username or password entered is incorrect.");
                    }
                }
            }
            else
            {
                DisplayError("The username or password field is empty");
            }
        }
    }
}
