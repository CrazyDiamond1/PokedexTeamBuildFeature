using System;
using System.Collections.Generic;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        public void DisplayError(string error)
        {
            tbxUsername.Clear();
            pbxPassword.Clear();
            pbxConfirmPassword.Clear();
            MessageBox.Show(error);
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(tbxUsername.Text) && !String.IsNullOrWhiteSpace(pbxPassword.Password) && !String.IsNullOrWhiteSpace(pbxConfirmPassword.Password))
            {
                if (pbxConfirmPassword.Password.Equals(pbxPassword.Password))
                {
                    using (var db = new PokedexTeamBuilderDBEntities())
                    {
                        var query = db.Users.Where(u => u.username == tbxUsername.Text);
                        var loadUser = query.ToList();
                        if (loadUser == null)
                        {
                            User newUser = new User()
                            {
                                username = tbxUsername.Text,
                                password = pbxPassword.Password
                            };
                            db.Users.Add(newUser);
                            db.SaveChanges();
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
                    DisplayError("The password and confirm password do not match.");
                }
            }
            else
            {
                DisplayError("One or more of the fields were left blank.");
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Views.Home home = new Views.Home();
            NavigationService.Navigate(home);
        }
    }
}
