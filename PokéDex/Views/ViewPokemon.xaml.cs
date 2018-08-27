using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ViewPokemon.xaml
    /// </summary>
    public partial class ViewPokemon : Page
    {
        private List<PokemonModel> res;
        Pokemon currentPokemon;
        MainWindow window;
        public ViewPokemon(Pokemon p)
        {
            InitializeComponent();
            window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            currentPokemon = p;
            LogInOutHandler();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            res = PokemonDAL.DAL.GetAllPokemon();
            pokéListBox.ItemsSource = res;
            FilterBox.SelectedIndex = 0;
        }

        private void LogInOutHandler()
        {
            MenuItem mI1 = new MenuItem();
            mI1.Header = "_LOGOUT";
            mI1.Click += LogOut;
            MenuItem mI2 = new MenuItem();
            mI2.Header = "_TEAMS";
            mI2.Click += TeamView;
            MainMenu.Items.Clear();
            MainMenu.Items.Add(mI1);
            MainMenu.Items.Add(mI2);
            MenuItem mI3 = new MenuItem();
            mI3.Header = "_User: " + window.username;
            mI3.HorizontalAlignment = HorizontalAlignment.Right;
            Menus.Items.Add(mI3);
        }

        private void TeamView(object sender, RoutedEventArgs e)
        {
            ViewTeam teamPage = new ViewTeam();
            this.NavigationService.Navigate(teamPage);
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            window.isLoggedIn = false;
            LogInOutHandler();
            Home homePage = new Home();
            this.NavigationService.Navigate(homePage);
        }

        private void pokéListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pokéListBox.SelectedIndex == -1)
                pokéListBox.SelectedIndex = 0;
            SlashBlock.Visibility = TypeBlock.Visibility = Visibility.Visible;
            FullGrid.DataContext = pokéListBox.SelectedItem;
            if (TypeBlock.Text == "")
                SlashBlock.Visibility = TypeBlock.Visibility = Visibility.Hidden;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text != "")
            {
                IEnumerable<PokemonModel> LinqRes = res;
                switch (FilterBox.SelectedIndex)
                {
                    case 0:
                        LinqRes = res.Where(p => p.name.ToLower().Contains(SearchBox.Text.ToLower()));
                        break;

                    case 1:
                        LinqRes = res.Where(p => p.id.ToString().Contains(SearchBox.Text));
                        break;

                    case 2:
                        var type1 = res.Where(p => p.type[0].ToLower().Contains(SearchBox.Text.ToLower()));
                        LinqRes = type1;
                        break;
                }
                pokéListBox.ItemsSource = LinqRes.ToList();
            }
            else
                pokéListBox.ItemsSource = res;
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBox.Text = "";
        }

        private void MoveTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LevelMovesBox.Visibility = EggMovesBox.Visibility = TmHmMovesBox.Visibility = Visibility.Hidden;
            switch (MoveTypeBox.SelectedIndex)
            {
                case 0:
                    LevelMovesBox.Visibility = Visibility.Visible;
                    break;
                case 1:
                    EggMovesBox.Visibility = Visibility.Visible;
                    break;
                case 2:
                    TmHmMovesBox.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
