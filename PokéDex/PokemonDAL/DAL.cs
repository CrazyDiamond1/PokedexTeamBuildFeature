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
        private static List<PokemonModel> allpokemons;
        public static List<PokemonModel> GetAllPokemon()
        {

            if (allpokemons == null)
            {
                allpokemons = new List<PokemonModel>();

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
        }

        public static PokemonModel GetPokemon(int nr)
        {
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
