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

namespace PokéDex
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<Pokemon> res;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            res = PokemonDAL.DAL.GetAllPokemon();
            pokéListBox.ItemsSource = res;
            FilterBox.SelectedIndex = 0;
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
                IEnumerable<Pokemon> LinqRes = res;
                switch (FilterBox.SelectedIndex)
                {
                    case 0:
                        LinqRes = res.Where(p => p.name.ToLower().Contains(SearchBox.Text.ToLower()));
                        break;

                    case 1:
                        LinqRes = res.Where(p => p.id.ToString().Contains(SearchBox.Text));
                        break;

                    case 2:
                        //LinqRes = res.Where(p => p.type[0].ToLower().Contains(SearchBox.Text.ToLower()) || p.type[1].ToLower().Contains(SearchBox.Text.ToLower()));
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
    public class DamageToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            string t = value as string;
            double dmg = System.Convert.ToDouble(t/*.Replace(".",",")*/);

            if (dmg < 1 && dmg != 0)
                return "Red";
            else if (dmg > 1)
                return "Green";
            else if (dmg == 1)
                return "Gray";
            else
                return "Black";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}