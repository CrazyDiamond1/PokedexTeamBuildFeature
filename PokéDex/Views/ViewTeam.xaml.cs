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
    public partial class ViewTeam : Page
    {
        List<Team> teams;
        Team currentTeam;
        MainWindow window;
        public ViewTeam()
        {
            InitializeComponent();
            teams = new List<Team>();
            window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            LogInOutHandler();
            LoadTeams();
        }
        
        private void LoadTeams(int num = 2)
        {
            using (var db = new PokedexTeamBuilderDBEntities())
            {
                foreach (var team in db.Teams.Where(t => t.UserID == db.Users.Where(u => u.username == window.username).FirstOrDefault().UserID))
                {
                    teams.Add(team);
                }
            }

            TeamImageGrid.Children.Clear();

            currentTeam = teams.Where(t => t.TeamID == num).FirstOrDefault();
            CurrentBox.Header = "TEAM ID: " + currentTeam.TeamID;
            CreateTeamDisplayImages();
            LoadTeamsIntoList();
        }

        private void DisplayDifferentTeam(object sender, RoutedEventArgs e)
        {
            LoadTeams(int.Parse((sender as Button).Content.ToString()));
        }

        private void LoadTeamsIntoList()
        {
            TeamsList.ItemsSource = teams;
            //foreach (var item in TeamsList.Items)
            //{
            //    item.
            //}
        }

        //Creates the images to display the user's currently selected team
        private void CreateTeamDisplayImages()
        {
            using (var db = new PokedexTeamBuilderDBEntities())
            {
                Team current = db.Teams.Where(t => t.TeamID == currentTeam.TeamID).FirstOrDefault();
                int i = 0;
                if (current.Pokemons.Count != 0)
                {
                    foreach (var poke in current.Pokemons)
                    {
                        Image img = new Image();
                        img.SetValue(Grid.ColumnProperty, i);
                        if (i == 1 || i == 4)
                        {
                            img.SetValue(Grid.RowProperty, 0);
                        }
                        else
                        {
                            img.SetValue(Grid.RowProperty, 1);
                        }
                        img.Source = new BitmapImage(new Uri(PokemonDAL.DAL.GetAllPokemon().Where(p => p.name == poke.PokemonName).FirstOrDefault().img, UriKind.Absolute));
                        img.MouseLeftButtonDown += ToPokemonView;
                        TeamImageGrid.Children.Add(img);
                        i++;
                    }
                }
                if (i < 6)
                {
                    Image img = new Image();
                    img.SetValue(Grid.ColumnProperty, i);
                    if (i == 1 || i == 4)
                    {
                        img.SetValue(Grid.RowProperty, 0);
                    }
                    else
                    {
                        img.SetValue(Grid.RowProperty, 1);
                    }
                    img.Source = new BitmapImage(new Uri("..\\Resources\\plus.png", UriKind.Relative));
                    img.MouseLeftButtonDown += ToPokemonView;
                    TeamImageGrid.Children.Add(img);
                }
            }
        }

        //Adds a team to the database under current users id
        private void AddTeam()
        {
            Team t = new Team();
            
            //t.Pokemons.Add(AddPokemonToTeam(351));
            //t.Pokemons.Add(AddPokemonToTeam(31));
            //t.Pokemons.Add(AddPokemonToTeam(51));
            //t.Pokemons.Add(AddPokemonToTeam(3));
            //t.Pokemons.Add(AddPokemonToTeam(5));
            //t.Pokemons.Add(AddPokemonToTeam(1));

            using (var db = new PokedexTeamBuilderDBEntities())
            {
                t.User = db.Users.Where(u => u.username == window.username).First();
                t.UserID = t.User.UserID;
                
                db.Teams.Add(t);
                db.SaveChanges();
            }
        }

        //Creates correct menuItems for the current page
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

        //Takes user to homeView
        private void HomeView(object sender, RoutedEventArgs e)
        {
            Home homePage = new Home();
            this.NavigationService.Navigate(homePage);
        }

        //Logs the user out
        private void LogOut(object sender, RoutedEventArgs e)
        {
            window.isLoggedIn = false;
            //LogInOutHandler();
            Home homePage = new Home();
            this.NavigationService.Navigate(homePage);
        }

        private void ToPokemonView(object sender, RoutedEventArgs e)
        {
            int teamIndex = TeamImageGrid.Children.IndexOf((Image)sender);
            using (var db = new PokedexTeamBuilderDBEntities())
            {
                ViewPokemon pokemonPage = new ViewPokemon(db.Teams.Where(curr => curr.TeamID == currentTeam.TeamID).FirstOrDefault().Pokemons.ElementAt(teamIndex));
                this.NavigationService.Navigate(pokemonPage);
            }
        }
    }
}
