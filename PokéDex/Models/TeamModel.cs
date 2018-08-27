using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokéDex.Models
{
    public class TeamModel
    {
        public List<PokemonModel> team;
        public string name;

        public TeamModel()
        {
            team = new List<PokemonModel>();
            name = "";
        }
        
        public void AddPokemon(PokemonModel pocketmonster)
        {
            if (pocketmonster != null)
            {
                team.Add(pocketmonster);
            }
        }



    }
}
