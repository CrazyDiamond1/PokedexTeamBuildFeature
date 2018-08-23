using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokéDex.Models
{
    public class Team
    {
        List<PokemonModel> team;

        public Team()
        {
            team = new List<PokemonModel>();
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
