using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using PokéDex.UserPages;

namespace PokéDex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {

        public bool isLoggedIn;
        public string username = "";
        public MainWindow()
        {
            InitializeComponent();
            currentView = SummaryBox;
            LogInOutHandler();
            
        }

        private List<PokemonModel> res;
        private bool isLoggedIn;
        private GroupBox currentView;

        //"Views"
        //pokedexView;
        //loginView;
        //teamView;
        //registerView;
        //addPokemonToTeam;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            res = PokemonDAL.DAL.GetAllPokemon();
            pokéListBox.ItemsSource = res;
            FilterBox.SelectedIndex = 0;
            isLoggedIn = true;
        }

        private void LogIn(object sender, RoutedEventArgs e)
        {
            isLoggedIn = true;
            LogInOutHandler();
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Login());
            //ViewChanger(TeamBox);

        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            isLoggedIn = false;
        }
    }
}