using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Newtonsoft.Json;

namespace PokéDex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGetpokemon_Click(object sender, RoutedEventArgs e)
        {
           var res= PokemonDAL.DAL.GetAllPokemon();
           MessageBox.Show("Retrieved " + res.Count + " pokemons");
           var s = PokemonDAL.DAL.GetPokemon(25);
           MessageBox.Show("Retrieved pokemon " + s.name);
        }
    }

}



