using System.Globalization;
using System.Windows.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokéDex
{

    public class Rootobject
    {
        [JsonProperty(PropertyName = "Property1")]
        public PokemonModel[] AllPokemons { get; set; }
    }

    public class PokemonModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string img { get; set; }
        public string[] type { get; set; }
        public Stats stats { get; set; }
        public Moves moves { get; set; }
        public Damages damages { get; set; }
        public Misc misc { get; set; }
        public string speedIndicator
        {
            get
            {
                if (Convert.ToInt32(stats.speed) < 75)
                    return Environment.CurrentDirectory + @"\Snail.png";
                else
                    return Environment.CurrentDirectory + @"\Hare.png";
            }
        }
        
    }

    public class Stats
    {
        public string hp { get; set; }
        public string attack { get; set; }
        public string defense { get; set; }
        public string spattack { get; set; }
        public string spdefense { get; set; }
        public string speed { get; set; }
    }

    public class Moves
    {
        public Level[] level { get; set; }
        public Tmhm[] tmhm { get; set; }
        public Egg[] egg { get; set; }
        public Tutor[] tutor { get; set; }
        public Gen34[] gen34 { get; set; }
    }

    public class Level
    {
        public string learnedat { get; set; }
        private string _name;
        public string name
        {
            get
            {
                TextInfo i = new CultureInfo("en-US", false).TextInfo;
                return i.ToTitleCase(_name);
            }
            set { _name = value; }
        }
        public string gen { get; set; }
    }

    public class Tmhm
    {
        private string _learnedat;
        public string learnedat
        {
            get
            {
                return _learnedat.ToUpper();
            }
            set { _learnedat = value; }
        }
        private string _name;
        public string name
        {
            get
            {
                TextInfo i = new CultureInfo("en-US", false).TextInfo;
                return i.ToTitleCase(_name);
            }
            set { _name = value; }
        }
        public string gen { get; set; }
    }

    public class Egg
    {
        private string _name;
        public string name
        {
            get
            {
                TextInfo i = new CultureInfo("en-US", false).TextInfo;
                return i.ToTitleCase(_name);
            }
            set { _name = value; }
        }
        public string gen { get; set; }
    }

    public class Tutor
    {
        private string _name;
        public string name
        {
            get
            {
                TextInfo i = new CultureInfo("en-US", false).TextInfo;
                return i.ToTitleCase(_name);
            }
            set { _name = value; }
        }
        public string gen { get; set; }
    }

    public class Gen34
    {
        private string _name;
        public string name
        {
            get
            {
                TextInfo i = new CultureInfo("en-US", false).TextInfo;
                return i.ToTitleCase(_name);
            }
            set { _name = value; }
        }
        public string method { get; set; }
    }

    public class Damages
    {
        public string normal { get; set; }
        public string fire { get; set; }
        public string water { get; set; }
        public string electric { get; set; }
        public string grass { get; set; }
        public string ice { get; set; }
        public string fight { get; set; }
        public string poison { get; set; }
        public string ground { get; set; }
        public string flying { get; set; }
        public string psychic { get; set; }
        public string bug { get; set; }
        public string rock { get; set; }
        public string ghost { get; set; }
        public string dragon { get; set; }
        public string dark { get; set; }
        public string steel { get; set; }
    }

    public class Misc
    {
        public Sex sex { get; set; }
        public Abilities abilities { get; set; }
        public string classification { get; set; }
        public string height { get; set; }
        public string weight { get; set; }
        public string capturerate { get; set; }
        public string eggsteps { get; set; }
        public string expgrowth { get; set; }
        public string happiness { get; set; }
        public string[] evpoints { get; set; }
        public string fleeflag { get; set; }
        public string entreeforestlevel { get; set; }
    }

    public class Sex
    {
        public string male { get; set; }
        public string female { get; set; }
    }

    public class Abilities
    {
        public string[] normal { get; set; }
        public string[] hidden { get; set; }
    }
}
