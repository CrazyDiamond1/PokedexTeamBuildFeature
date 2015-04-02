using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PokéDex.PokemonDAL
{
    class DAL
    {
        private static List<Pokemon> allpokemons;
        public static List<Pokemon> GetAllPokemon()
        {

            if (allpokemons == null)
            {
                allpokemons = new List<Pokemon>();

                using (StreamReader sr = new StreamReader("Resources\\PokemonData.txt"))
                {
                    string jsondata = sr.ReadToEnd();
                    Rootobject Jdata = JsonConvert.DeserializeObject<Rootobject>(jsondata);

                    foreach (var pokemon in Jdata.AllPokemons)
                    {

                        allpokemons.Add(pokemon);

                    }

                }

            }
            return allpokemons;



            return null;
        }

        public static Pokemon GetPokemon(int nr)
        {
            Pokemon pokemon;

            if (allpokemons == null)
            { GetAllPokemon(); }

            var single = allpokemons.Where(p => int.Parse(p.id) == nr).FirstOrDefault();
            if (single != null)
                return single;
            else
                throw new PokeMonNotFoundException();
        }

        public class PokeMonNotFoundException : Exception
        {

        }

    }
}
