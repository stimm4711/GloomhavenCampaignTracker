using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GloomhavenCampaignTracker.Droid.Data.ConversionClasses
{
    internal class JSONImporter
    {
        internal static T LoadJson<T>(Stream asset)
        {
            T items;
            using (StreamReader r = new StreamReader(asset))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<T>(json);
            }

            if (items != null)
                return items;
            else
                return default(T);
        }
    }

    public class CharacterClassAbility
    {
        public string abilityName { get; set; }
        public int referenceNumber { get; set; }
        public int level { get; set; }
    }

    public class CharacterClassPerk
    {
        public string Perktext { get; set; }
        public int Checkboxnumber { get; set; }
    }

    public class CharacterClass
    {
        public string Classname_long { get; set; }
        public int Classnumber { get; set; }
        public string Classname { get; set; }
        public string ClassnameShorty { get; set; }
        public int ID_Pack { get; set; }
        public int Handsize { get; set; }
        public int Hitpoints { get; set; }

        public List<CharacterClassAbility> Abilities { get; set; }

        public List<CharacterClassPerk> Perks { get; set; }
    }

    public class CLasses
    {
        public List<CharacterClass> characterclasses { get; set; }
    }
}