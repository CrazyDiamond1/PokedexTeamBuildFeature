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

        private void LoadTeams(int num = 0)
        {
            using (var db = new PokedexTeamBuilderDBEntities())
            {
                foreach (var team in db.Teams.Where(t => t.UserID == db.Users.Where(u => u.username == window.username).FirstOrDefault().UserID))
                {
                    teams.Add(team);
                }
            }

            TeamImageGrid.Children.Clear();
            if (num == 0)
            {
                currentTeam = teams.Where(t => t.TeamID == t.TeamID).FirstOrDefault();
            }
            else{

                currentTeam = teams.Where(t => t.TeamID == num).FirstOrDefault();
            }
            if (currentTeam != null)
            {
                CurrentBox.Header = "TEAM ID: " + currentTeam.TeamID;
                CreateTeamDisplayImages();
                LoadTeamsIntoList();

            }

            Button addButton = new Button();
            addButton.Content = "Add Team";
            addButton.SetCurrentValue(Grid.ColumnProperty, 0);
            TeamImageGrid.Children.Add(addButton);
            addButton.Margin = new Thickness(20, 30, 20, 30);
            addButton.Click += AddTeam;
            Button deleteButton = new Button();
            deleteButton.Content = "Delete Team";
            deleteButton.SetCurrentValue(Grid.ColumnProperty, 5);
            deleteButton.Margin = new Thickness(20, 30, 20, 30);
            deleteButton.Click += DeletePokemon;
            TeamImageGrid.Children.Add(deleteButton);

        }

        //Button click function used to display the selected team
        private void DisplayDifferentTeam(object sender, RoutedEventArgs e)
        {
            LoadTeams(int.Parse((sender as Button).Content.ToString()));
        }

        //Loads teams into the listbox
        private void LoadTeamsIntoList()
        {
            TeamsList.ItemsSource = teams;
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
        private void AddTeam(object sender, RoutedEventArgs e)
        {
            Team t = new Team();

            using (var db = new PokedexTeamBuilderDBEntities())
            {
                t.User = db.Users.Where(u => u.username == window.username).First();
                t.UserID = t.User.UserID;

                db.Teams.Add(t);
                db.SaveChanges();
            }

            ViewTeam teamPage = new ViewTeam();
            this.NavigationService.Navigate(teamPage);

        }

        private void DeletePokemon(object sender, RoutedEventArgs e)
        {
            using (var db = new PokedexTeamBuilderDBEntities())
            {
                var query = db.Pokemons.Where(p => p.TeamID == currentTeam.TeamID);
                List<Pokemon> toRemove = query.ToList();
                foreach (var pokemon in toRemove)
                {
                    db.Pokemons.Remove(pokemon);
                }
                db.SaveChanges();
                Team team = db.Teams.Where(t => t.TeamID == currentTeam.TeamID).FirstOrDefault();
                db.Teams.Remove(team);
                db.SaveChanges();
            }

            ViewTeam teamPage = new ViewTeam();
            this.NavigationService.Navigate(teamPage);
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
            //MenuItem mI4 = new MenuItem();
            //mI4.Header = "_DELETE_ACCOUNT";
            //mI4.Click += DeleteAccount;
            //mI4.HorizontalAlignment = HorizontalAlignment.Right;
            //mI3.Items.Add(mI4);
        }

        private void DeleteAccount(object sender, RoutedEventArgs e)
        {
            using (var db = new PokedexTeamBuilderDBEntities())
            {
                var queryUser = db.Users.Where(u => u.username == window.username).FirstOrDefault();
                var queryTeams = db.Teams.Where(t => t.UserID == queryUser.UserID);
                var teamsToDelete = queryTeams.ToList();
                List<Pokemon> queryPokemon;
                teamsToDelete.ForEach(t =>
                {
                    queryPokemon = db.Pokemons.Where(p => p.TeamID == t.TeamID).ToList();
                    queryPokemon.ForEach(p =>
                    {
                        db.Pokemons.Remove(p);
                    });
                    db.Teams.Remove(t);
                });

            }
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
            ViewPokemon pokemonPage;
            using (var db = new PokedexTeamBuilderDBEntities())
            {
                if (db.Teams.Where(curr => curr.TeamID == currentTeam.TeamID).FirstOrDefault().Pokemons.Count == 6)
                {
                    pokemonPage = new ViewPokemon(db.Teams.Where(curr => curr.TeamID == currentTeam.TeamID).FirstOrDefault(), db.Teams.Where(curr => curr.TeamID == currentTeam.TeamID).FirstOrDefault().Pokemons.ElementAt(teamIndex));
                }
                else
                {
                    pokemonPage = new ViewPokemon(db.Teams.Where(curr => curr.TeamID == currentTeam.TeamID).FirstOrDefault(), null);
                }
                this.NavigationService.Navigate(pokemonPage);
            }
        }
    }
}
