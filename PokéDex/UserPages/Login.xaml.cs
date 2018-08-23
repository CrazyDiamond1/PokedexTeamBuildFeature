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
        public Login()
        {
            InitializeComponent();
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
                        Page home = new Page();
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
