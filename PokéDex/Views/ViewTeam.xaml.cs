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
            LoadTeam();
        }

        //private TeamModel TempTeam()
        //{
        //    Team t = new Team();
        //    Pokemon p1 = new Pokemon();
        //    t.Pokemons.Add(PokemonDAL.DAL.GetPokemon(545));
        //    t.AddPokemon(PokemonDAL.DAL.GetPokemon(6));
        //    t.AddPokemon(PokemonDAL.DAL.GetPokemon(9));
        //    t.AddPokemon(PokemonDAL.DAL.GetPokemon(542));
        //    t.AddPokemon(PokemonDAL.DAL.GetPokemon(545));
        //    t.AddPokemon(PokemonDAL.DAL.GetPokemon(530));
        //    t.name = t.TeamID;
        //    return t;
        //}

        private void LoadTeam()
        {
            CreateAddInitialTeam();
            currentTeam = teams[0];

            CurrentBox.Header = teams[0].TeamID;
            p1.Source = new BitmapImage(new Uri(PokemonDAL.DAL.GetPokemon(currentTeam.Pokemons.ElementAt(0).PokemonID).img, UriKind.Absolute));
            p2.Source = new BitmapImage(new Uri(PokemonDAL.DAL.GetPokemon(currentTeam.Pokemons.ElementAt(1).PokemonID).img, UriKind.Absolute));
            p3.Source = new BitmapImage(new Uri(PokemonDAL.DAL.GetPokemon(currentTeam.Pokemons.ElementAt(2).PokemonID).img, UriKind.Absolute));
            p4.Source = new BitmapImage(new Uri(PokemonDAL.DAL.GetPokemon(currentTeam.Pokemons.ElementAt(3).PokemonID).img, UriKind.Absolute));
            p5.Source = new BitmapImage(new Uri(PokemonDAL.DAL.GetPokemon(currentTeam.Pokemons.ElementAt(4).PokemonID).img, UriKind.Absolute));
            p6.Source = new BitmapImage(new Uri(PokemonDAL.DAL.GetPokemon(currentTeam.Pokemons.ElementAt(5).PokemonID).img, UriKind.Absolute));
        }


        private void CreateAddInitialTeam()
        {
            Team t = new Team();
            
            t.Pokemons.Add(AddPokemonToTeam(351));
            t.Pokemons.Add(AddPokemonToTeam(31));
            t.Pokemons.Add(AddPokemonToTeam(51));
            t.Pokemons.Add(AddPokemonToTeam(3));
            t.Pokemons.Add(AddPokemonToTeam(5));
            t.Pokemons.Add(AddPokemonToTeam(1));

            teams.Add(t);

            //using (var db = new PokedexTeamBuilderDBEntities())
            //{
            //    Team newTeam = t;
            //    newTeam.User = db.Users.Where(u => u.username == window.username).First();
            //    newTeam.UserID = newTeam.User.UserID;

            //    db.Teams.Add(t);
            //    db.SaveChanges();
            //}
        }

        private Pokemon AddPokemonToTeam(int pokemonID)
        {
            Pokemon pok = new Pokemon();
            pok.PokemonID = pokemonID;
            pok.PokemonName = PokemonDAL.DAL.GetPokemon(pok.PokemonID).name;
            return pok;
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
