using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokéDex.Models
{
    public class Team
    {
        List<Pokemon> team;

        public Team()
        {
            team = new List<Pokemon>();
        }
        
        public void AddPokemon(Pokemon pocketmonster)
        {
            if (pocketmonster != null)
            {
                team.Add(pocketmonster);
            }
        }



    }
}
