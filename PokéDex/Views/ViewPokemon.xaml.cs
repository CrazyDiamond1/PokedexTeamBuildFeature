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
        Pokemon pokemonToEdit;
        Team previousTeam;
        MainWindow window;
        List<string> movesList;

        public ViewPokemon(Team team, Pokemon p = null)
        {
            InitializeComponent();
            window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            pokemonToEdit = p;
            previousTeam = team;
            LogInOutHandler();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            res = PokemonDAL.DAL.GetAllPokemon();
            pokéListBox.ItemsSource = res;
            if (pokemonToEdit != null)
            {
                pokéListBox.SelectedIndex = res.IndexOf(res.Where(p => p.name == pokemonToEdit.PokemonName).FirstOrDefault());

            }
            LoadAllMoveBoxes();
        }

        private List<string> GetMoves()
        {
            List<string> m = new List<string>();

            PokemonModel pokemonQuery = res.Where(p => p.name == (pokéListBox.Items.GetItemAt(pokéListBox.SelectedIndex) as PokemonModel).name).FirstOrDefault();

            Moves moves = res.Where(p => p.name == pokemonQuery.name).FirstOrDefault().moves;

            foreach (var move in moves.level)
            {
                m.Add(move.name);
            }

            foreach (var move in moves.egg)
            {
                m.Add(move.name);
            }

            foreach (var move in moves.tmhm)
            {
                m.Add(move.name);
            }

            foreach (var move in moves.tutor)
            {
                m.Add(move.name);
            }

            foreach (var move in moves.gen34)
            {
                m.Add(move.name);
            }

            return m;
        }

        private void LoadAllMoveBoxes()
        {
            movesList = GetMoves();
            Move1Box.ItemsSource = movesList;
            Move2Box.ItemsSource = movesList;
            Move3Box.ItemsSource = movesList;
            Move4Box.ItemsSource = movesList;
            if (pokemonToEdit != null)
            {

                Move1Box.SelectedValue = pokemonToEdit.move1;
                Move2Box.SelectedValue = pokemonToEdit.move2;
                Move3Box.SelectedValue = pokemonToEdit.move3;
                Move4Box.SelectedValue = pokemonToEdit.move4;
            }
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

        private void SavePokemon(object sender, RoutedEventArgs e)
        {
            Pokemon p = new Pokemon();
            PokemonModel pokemonQuery = res.Where(pok => pok.name == (pokéListBox.Items.GetItemAt(pokéListBox.SelectedIndex) as PokemonModel).name).FirstOrDefault();
            p.move1 = Move1Box.SelectedValue.ToString();
            p.move2 = Move2Box.SelectedValue.ToString();
            p.move3 = Move3Box.SelectedValue.ToString();
            p.move4 = Move4Box.SelectedValue.ToString();
            p.TeamID = previousTeam.TeamID;
            p.PokemonName = pokemonQuery.name;

            using (var db = new PokedexTeamBuilderDBEntities())
            {
                var query = db.Teams.Where(t => t.TeamID == previousTeam.TeamID).FirstOrDefault();
                if (pokemonToEdit != null)
                {
                    query.Pokemons.Remove(db.Pokemons.Where(pok => pok.PokemonID == pokemonToEdit.PokemonID).FirstOrDefault());
                }
                query.Pokemons.Add(p);

                db.SaveChanges();
            }

            ViewTeam teamPage = new ViewTeam();
            this.NavigationService.Navigate(teamPage);
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
            //if (pokéListBox.SelectedIndex == -1)
            //    pokéListBox.SelectedIndex = 0;
            FullGrid.DataContext = pokéListBox.SelectedItem;
            LoadAllMoveBoxes();
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
    }
}
