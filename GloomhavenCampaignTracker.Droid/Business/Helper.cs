using System.Collections.Generic;
using System.Linq;
using System.Text;
using Math = System.Math;
using Java.Lang.Reflect;
using Android.Widget;
using System;

namespace GloomhavenCampaignTracker.Business
{
    public enum UnlockTypes 
    {
        ItemDesign = 1,
        Class = 2
    }
    public enum EventTypes 
    {
        CityEvent = 1,
        RoadEvent = 2
    }
    public enum DetailFragmentTypes 
    {
        GlobalAchievements = 1,
        UnlockedScenarios = 2,
        Itemstore = 3,
        Roadevents = 4,
        Cityevents = 5,
        CampaignSelection = 8,
        PartyAchievements = 9,
        PartySelection = 10,
        PartyMember = 11,
        CharacterDetail = 12,
        Characters = 13,
        Settings = 14,
        Support = 15,
        Releasenotes = 16,
        EnvelopeXUnlock = 17
    }
    
    public class Helper
    {
        private static readonly Dictionary<int, int> ProsperityLevelSteps = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> ReputationShopPriceModifierSteps = new Dictionary<int, int>();
        private static readonly Dictionary<int, Tuple<string, string>> RegionIdToShortyAndName = new Dictionary<int, Tuple<string, string>>();
        private static readonly Dictionary<int, int> ScenariolevelGoldconversion = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> ScenariolevelTrapDamage = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> ScenariolevelBonusexperience = new Dictionary<int, int>();
        private static readonly List<int> ProsperityForDonation = new List<int>(new int[] { 100, 150, 200,250,300,350,400,500,600,700,800,900,1000 });

        private static void InitReputation()
        {
            ReputationShopPriceModifierSteps.Add(0, 0);
            ReputationShopPriceModifierSteps.Add(1, 0);
            ReputationShopPriceModifierSteps.Add(2, 0);
            ReputationShopPriceModifierSteps.Add(3, 1);
            ReputationShopPriceModifierSteps.Add(4, 1);
            ReputationShopPriceModifierSteps.Add(5, 1);
            ReputationShopPriceModifierSteps.Add(6, 1);
            ReputationShopPriceModifierSteps.Add(7, 2);
            ReputationShopPriceModifierSteps.Add(8, 2);
            ReputationShopPriceModifierSteps.Add(9, 2);
            ReputationShopPriceModifierSteps.Add(10, 2);
            ReputationShopPriceModifierSteps.Add(11, 3);
            ReputationShopPriceModifierSteps.Add(12, 3);
            ReputationShopPriceModifierSteps.Add(13, 3);
            ReputationShopPriceModifierSteps.Add(14, 3);
            ReputationShopPriceModifierSteps.Add(15, 4);
            ReputationShopPriceModifierSteps.Add(16, 4);
            ReputationShopPriceModifierSteps.Add(17, 4);
            ReputationShopPriceModifierSteps.Add(18, 4);
            ReputationShopPriceModifierSteps.Add(19, 5);
            ReputationShopPriceModifierSteps.Add(20, 5);
        }

        private static void InitProsperty()
        {
            ProsperityLevelSteps.Add(1, 1);
            ProsperityLevelSteps.Add(2, 5);
            ProsperityLevelSteps.Add(3, 10);
            ProsperityLevelSteps.Add(4, 16);
            ProsperityLevelSteps.Add(5, 23);
            ProsperityLevelSteps.Add(6, 31);
            ProsperityLevelSteps.Add(7, 40);
            ProsperityLevelSteps.Add(8, 51);
            ProsperityLevelSteps.Add(9, 65);
        }

        private static void InitRegions()
        {
            RegionIdToShortyAndName.Add(1, new Tuple<string, string>("GLO", "Gloomhaven"));
            RegionIdToShortyAndName.Add(2, new Tuple<string, string>("DF", "Dagger Forest"));
            RegionIdToShortyAndName.Add(3, new Tuple<string, string>("CM", "Copperneck Mount."));
            RegionIdToShortyAndName.Add(4, new Tuple<string, string>("LS", "Lingering Swamp"));
            RegionIdToShortyAndName.Add(5, new Tuple<string, string>("CW", "Corpsewood"));
            RegionIdToShortyAndName.Add(6, new Tuple<string, string>("WM", "Watcher Mount."));
            RegionIdToShortyAndName.Add(7, new Tuple<string, string>("MS", "Misty Sea"));

        }

        private static void InitGoldconversion()
        {
            ScenariolevelGoldconversion.Add(0, 2);
            ScenariolevelGoldconversion.Add(1, 2);
            ScenariolevelGoldconversion.Add(2, 3);
            ScenariolevelGoldconversion.Add(3, 3);
            ScenariolevelGoldconversion.Add(4, 4);
            ScenariolevelGoldconversion.Add(5, 4);
            ScenariolevelGoldconversion.Add(6, 5);
            ScenariolevelGoldconversion.Add(7, 6);
        }

        private static void InitScenariolevelTrapdamage()
        {
            ScenariolevelTrapDamage.Add(0, 2);
            ScenariolevelTrapDamage.Add(1, 3);
            ScenariolevelTrapDamage.Add(2, 4);
            ScenariolevelTrapDamage.Add(3, 5);
            ScenariolevelTrapDamage.Add(4, 6);
            ScenariolevelTrapDamage.Add(5, 7);
            ScenariolevelTrapDamage.Add(6, 8);
            ScenariolevelTrapDamage.Add(7, 9);
        }

        private static void InitScenariolevelBonusXP()
        {
            ScenariolevelBonusexperience.Add(0, 4);
            ScenariolevelBonusexperience.Add(1, 6);
            ScenariolevelBonusexperience.Add(2, 8);
            ScenariolevelBonusexperience.Add(3, 10);
            ScenariolevelBonusexperience.Add(4, 12);
            ScenariolevelBonusexperience.Add(5, 14);
            ScenariolevelBonusexperience.Add(6, 16);
            ScenariolevelBonusexperience.Add(7, 18);
        }

        public static int GetStepsToNextLevel(int prosperityLevel)
        {
            if (!ProsperityLevelSteps.Any())
            {
                InitProsperty();
            }
            return ProsperityLevelSteps.ContainsKey(prosperityLevel + 1) ? ProsperityLevelSteps[prosperityLevel + 1] : 0;
        }

        public static int GetProsperityLevel(int cityProsperity)
        {
            if (!ProsperityLevelSteps.Any())
            {
                InitProsperty();
            }
            var kvp = ProsperityLevelSteps.Where(x => x.Value <= cityProsperity);
            var keyValuePairs = kvp as IList<KeyValuePair<int, int>> ?? kvp.ToList();
            return keyValuePairs.Any() ? keyValuePairs.Max(x => x.Key) : 9;
        }

        public static string GetProsperityLevelUnlockedItems(int prosperity)
        {
            var propLevel = GetProsperityLevel(prosperity);
            var rangeEnd = ((propLevel + 1) * 7);
            return "000 - 0" + rangeEnd;
        }

        internal static int GetGoldConverionForScenarioLevel(int sl)
        {
            
            if(!ScenariolevelGoldconversion.Any())
            {
                InitGoldconversion();
            }

            if(ScenariolevelGoldconversion.ContainsKey(sl))
            {
                return ScenariolevelGoldconversion[sl];
            }

            return 0;
        }

        internal static int GetTrapdamageForScenarioLevel(int sl)
        {

            if (!ScenariolevelTrapDamage.Any())
            {
                InitScenariolevelTrapdamage();
            }

            if (ScenariolevelTrapDamage.ContainsKey(sl))
            {
                return ScenariolevelTrapDamage[sl];
            }

            return 0;
        }

        internal static int GetBonusXPForScenarioLevel(int sl)
        {

            if (!ScenariolevelBonusexperience.Any())
            {
                InitScenariolevelBonusXP();
            }

            if (ScenariolevelBonusexperience.ContainsKey(sl))
            {
                return ScenariolevelBonusexperience[sl];
            }

            return 0;
        }

        public static int GetShopPriceModifier(int partyreputation)
        {
            int modifier = 0;

            if (!ReputationShopPriceModifierSteps.Any())
            {
                InitReputation();
            }

            var absReputation = Math.Abs(partyreputation);

            if (ReputationShopPriceModifierSteps.ContainsKey(absReputation))
            {
                var modi = ReputationShopPriceModifierSteps[absReputation];
                if (partyreputation > 0)
                {
                    modifier = -modi;
                }
                else
                {
                    modifier = modi;
                }                
            }
            
            return modifier;
        }


        public static int GetNextDonationValueForProsperity(int currentDonations)
        {          
            if (currentDonations >= 1000) return 0;

            if (currentDonations < 400) return Math.Max(100, ((currentDonations / 50) + 1) * 50);
  
            return ((currentDonations / 100) + 1) * 100;

        }

        public static string IntListToString(IEnumerable<int> list)
        {
            var enumerable = list as IList<int> ?? list;
            if (!enumerable.Any()) return "";

            var sb = new StringBuilder();
            foreach (var i in enumerable)
            {
                sb.Append(i.ToString());
                sb.Append(",");
            }

            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public static List<int> StringToIntList(string commaseparatedIntString)
        {
            var set = new List<int>();

            if (string.IsNullOrEmpty(commaseparatedIntString)) return set;

            foreach (var intstring in commaseparatedIntString.Split(','))
            {
                if (int.TryParse(intstring, out var integer))
                {
                    set.Add(integer);
                }
            }

            return set;
        }

        public static void ForcePopupmenuToShowIcons(PopupMenu menu)
        {
            Field field = menu.Class.GetDeclaredField("mPopup");
            field.Accessible = true;
            Java.Lang.Object menuPopupHelper = field.Get(menu);
            Method setForceIcons = menuPopupHelper.Class.GetDeclaredMethod("setForceShowIcon", Java.Lang.Boolean.Type);
            setForceIcons.Invoke(menuPopupHelper, true);
        }

        public static List<string> Patronnames()
        {
            var names = new List<string>()
            {
                "Guido \"Fox Andersson\" Marzucchi",
                "Gonzalo Herreros",
                "Jamie Cottrell",
                "Marek Picha",
                "Robert Sigrist",
                "Brian Hazard (Game Spasm)",
                "John Finch",
                "Yong Siong Oon",
                "Jonathan Robelia",
                "Jim Flemming",
                "Ryan Marriott",
                "Christian Beaman",
                "Carson Martinez",
                "Anton Kuchman",
                "Mihai Milea",
                "Eric Lomas",
                "Jan Andreas Krahl",
                "David Klein",
                "Kevin Alexander",
                "Michael Street",
                "Lorenz Lißeck",
                "Kevin Pham",
                "Jeffrey Hickey",
                "Brainy Zombie Games",
                "Rob Rivera",
                "Andrew Stoute",
                "Jan Brockmann",
                "Ethan Chua"
            };            

            return names;
        }

        public static string GetRegionName(int scenarioRegionId)
        {
            if (!RegionIdToShortyAndName.Any()) InitRegions();

            if (RegionIdToShortyAndName.ContainsKey(scenarioRegionId))
            {
                var name = RegionIdToShortyAndName[scenarioRegionId];
                return name.Item2;
            }
            return "";
        }

        public static string GetRegionShorty(int scenarioRegionId)
        {
            if (!RegionIdToShortyAndName.Any()) InitRegions();

            if (RegionIdToShortyAndName.ContainsKey(scenarioRegionId))
            {
                var name = RegionIdToShortyAndName[scenarioRegionId];
                return name.Item1;
            }
            return "";
        }
    }
}

    