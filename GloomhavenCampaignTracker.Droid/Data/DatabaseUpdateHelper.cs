using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Business;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;

namespace Data
{
    internal class DatabaseUpdateHelper
    {
        private enum VersionTime { Earlier = -1 }
        public static Version Dbversion { get; } = new Version(1, 4, 11);
        public static SQLiteConnection Connection => GloomhavenDbHelper.Connection;

        internal static void CheckForUpdates(DL_GlommhavenSettings currentDbVersion)
        {
            var old = Version.Parse(currentDbVersion.Value);
            if ((VersionTime)old.CompareTo(Dbversion) == VersionTime.Earlier)
            {
                if ((VersionTime)old.CompareTo(new Version(1, 3, 4)) == VersionTime.Earlier)
                {
                    FixItem33ItemCategorie();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 3, 5)) == VersionTime.Earlier)
                {
                    AddPartyAchievementSunblessed();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 3, 11)) == VersionTime.Earlier)
                {
                    GloomhavenDbHelper.FillPersonalQuestCounters();
                    FillCharacterPersonalQuestCounters();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 3, 12)) == VersionTime.Earlier && old.Build >= 11)
                {
                    UpdatePersonalQuestCountersWithScenarios();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 3, 15)) == VersionTime.Earlier)
                {
                    UpdatePersonalQuestCountersFor520();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 3, 17)) == VersionTime.Earlier)
                {
                    FixNameOfScenario31();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 3, 18)) == VersionTime.Earlier)
                {
                    FixNameOfScenario54();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 3, 19)) == VersionTime.Earlier)
                {
                    FixNameOfPQ533();
                    FixNameOfGA130();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 3, 20)) == VersionTime.Earlier)
                {
                    FixScenarioRegionOf61();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 1)) == VersionTime.Earlier)
                {
                    FixScenarioUnlockedScenariosOf39();
                }

                // 1.4.2 = Added Bladeswarm perks

                if ((VersionTime)old.CompareTo(new Version(1, 4, 4)) == VersionTime.Earlier)
                {
                    FixScenario39unlockedScenarios();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 6)) == VersionTime.Earlier)
                {
                    FixQuatermasterStunPerk();
                    FixElementalistStunPerk();
                    FixNameOfScenario83();
                    AddItem151();
                }
                
                if ((VersionTime)old.CompareTo(new Version(1, 4, 7)) == VersionTime.Earlier)
                {
                    FixSawHealPerk();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 8)) == VersionTime.Earlier)
                {
                    AddClasses();
                    AddDivinerClass();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 10)) == VersionTime.Earlier)
                {
                    AddContentOfID();
                    AddFCGlobalAchievements();
                    AddFCPartyAchievements();
                    AddFCScenarios();
                    AddFCItems();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 11)) == VersionTime.Earlier)
                {
                    FixScenarioRegions();
                }

                currentDbVersion.Value = Dbversion.ToString();
                GloomhavenSettingsRepository.InsertOrReplace(currentDbVersion);
            }
        }

        private static void AddFCItems()
        {           
            Connection.BeginTransaction();
            try
            {       
                var items = ItemRepository.Get();
                SaveFCItems(items);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        internal static void SaveFCItems(List<DL_Item> items)
        {
            /* Categories
             * 
             * 0 = helmet
             * 1= armor
             * 2= one hand
             * 3= two hand
             * 4= boots
             * 5= small
             * 
             */

            SaveItem(itemnumber: 152, itemname: "Ring of Duality", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 153, itemname: "Minor Antidote", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 25, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 154, itemname: "Major Antidote", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 155, itemname: "Curseward Armor", itemcategorie: 1, itemcount: 1, itemtext: "", itemprice: 75, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 156, itemname: "Elemental Claymore", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 157, itemname: "Ancient Bow", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 158, itemname: "Rejuvenation Greaves", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 35, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 159, itemname: "Scroll of Haste", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 160, itemname: "Cutpurse Dagger", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 45, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 161, itemname: "Throwing Axe", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 35, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 162, itemname: "Rift Device", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 163, itemname: "Crystal Tiara", itemcategorie: 0, itemcount: 1, itemtext: "", itemprice: 75, prosperitylevel: 150, items, 2);
            SaveItem(itemnumber: 164, itemname: "Basin of Prophecy", itemcategorie: 3, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items, 2);
        }

        private static void AddContentOfID()
        {
            Connection.BeginTransaction();
            try
            {
                // Content of 
                // 1 = Gloomhaven
                // 2 = Forgotten Circles
             
                // Update classes
                var classes = ClassRepository.Get();
                foreach(var cl in classes)
                {
                    if (cl.ClassId < 18)
                        cl.ContentOfPack = 1;
                    if (cl.ClassId == 18)
                        cl.ContentOfPack = 2;

                    ClassRepository.InsertOrReplace(cl);
                }

                // Update AchievementTypes
                var gachievement = AchievementTypeRepository.Get();
                var fcAcheievements = new List<int> { 17, 181, 182, 183, 184, 191, 192, 193, 2001, 2002, 2003, 2004, 21, 22 };
                foreach (var ga in gachievement)
                {
                    if (fcAcheievements.Contains(ga.InternalNumber))
                        ga.ContentOfPack = 2;
                    else
                        ga.ContentOfPack = 1;

                    AchievementTypeRepository.InsertOrReplace(ga);
                }

                // Update Items
                var items = ItemRepository.GetAllWithChildren();               
                // fc items = 152 -164
                foreach (var item in items)
                {
                    if (item.Itemnumber < 152)
                        item.ContentOfPack = 1;
                    else
                        item.ContentOfPack = 2;

                    ItemRepository.InsertOrReplace(item);
                }

                // update party achievements
                var paachievements = PartyAchievementRepository.Get();
                foreach (var pa in paachievements)
                {
                    if (pa.InternalNumber < 28)
                        pa.ContentOfPack = 1;
                    else
                        pa.ContentOfPack = 2;

                    PartyAchievementRepository.InsertOrReplace(pa);
                }

                // update scenarios
                var scenarios = ScenarioRepository.Get();
                foreach (var s in scenarios)
                {
                    if (s.Scenarionumber < 96)
                        ScenarioRepository.UpdateScenarioContentOf(s.Scenarionumber,1);  
                    else
                        ScenarioRepository.UpdateScenarioContentOf(s.Scenarionumber, 2);                    
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void AddFCPartyAchievements()
        {
            Connection.BeginTransaction();
            try
            {
                SaveFCPartyAchievements();
                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        internal static void SaveFCPartyAchievements()
        {
            var paments = PartyAchievementRepository.Get();

            SavePartyAchievement(29, "Hunting the Hunter", paments, contentOfPack: 2);
            SavePartyAchievement(30, "Guard Detail", paments, contentOfPack: 2);
            SavePartyAchievement(31, "Dimensional Equilibrium", paments, contentOfPack: 2);
            SavePartyAchievement(32, "Angels of Death", paments, contentOfPack: 2);
            SavePartyAchievement(33, "Diamara's Aid", paments, contentOfPack: 2);
            SavePartyAchievement(34, "Hunted Prey", paments, contentOfPack: 2);
            SavePartyAchievement(35, "Accomplices", paments, contentOfPack: 2);
            SavePartyAchievement(36, "Saboteurs", paments, contentOfPack: 2);
            SavePartyAchievement(37, "Custodians", paments, contentOfPack: 2);
            SavePartyAchievement(38, "A Strongbox", paments, contentOfPack: 2);
            SavePartyAchievement(39, "Opportunists", paments, contentOfPack: 2);
        }

        internal static void SavePartyAchievement(int number, string name, List<DL_PartyAchievement> existingPAAchievements = null, int contentOfPack = 1)
        {
            if (existingPAAchievements != null && existingPAAchievements.Any(x => x.Name == name)) return;

            var achievement = new DL_PartyAchievement
            {
                Name = name,
                InternalNumber = number,
                ContentOfPack = contentOfPack
            };

            PartyAchievementRepository.InsertOrReplace(achievement);
        }

        private static void AddFCScenarios()
        {
            Connection.BeginTransaction();
            try
            {                
                SaveFCScenarios();
                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        internal static void SaveFCScenarios()
        {
            var scenarios = ScenarioRepository.Get();

            SaveScenario("Unexpected Visitors", 96, requiredGlobalAchievements: "14", contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("Lore Untold", 97,  unlockedScenarioIdsCommaSeparated: "98,99,100,101", contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("Past in Flames", 98,  unlockedScenarioIdsCommaSeparated: "102,103", contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("Aftershocks", 99,  unlockedScenarioIdsCommaSeparated: "104,105", contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("Shifting Gears", 100,  unlockedScenarioIdsCommaSeparated: "106,107", contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("Shrouded Crypt", 101,  unlockedScenarioIdsCommaSeparated: "108,109", contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("Bazaar of Knowledge", 102,  requiredGlobalAchievements: "182,183,184", unlockedScenarioIdsCommaSeparated: "110", contentOfPack: 2, existingScenarios: scenarios); // OR
            SaveScenario("Where It Is Needed", 103,  requiredGlobalAchievements: "182,183,184", unlockedScenarioIdsCommaSeparated: "110", contentOfPack: 2, existingScenarios: scenarios); // OR
            SaveScenario("A Gaping Wound", 104,  requiredGlobalAchievements: "182,183,184", unlockedScenarioIdsCommaSeparated: "111", contentOfPack: 2, existingScenarios: scenarios); // OR
            SaveScenario("Monstrosities of a Cult", 105,  requiredGlobalAchievements: "182,183,184", blockingPartyAchievements: "29", unlockedScenarioIdsCommaSeparated: "111", contentOfPack: 2, existingScenarios: scenarios); // OR (req)
            SaveScenario("Intricate Work", 106,  requiredGlobalAchievements: "182,183,184", contentOfPack: 2, existingScenarios: scenarios); // OR
            SaveScenario("Mechanical Genius", 107,  requiredGlobalAchievements: "182,183,184", contentOfPack: 2, existingScenarios: scenarios); // OR
            SaveScenario("Prologue to the End", 108,  requiredGlobalAchievements: "182,183,184", contentOfPack: 2, existingScenarios: scenarios); // OR
            SaveScenario("Epilogue of a War", 109,  requiredGlobalAchievements: "182,183,184", unlockedScenarioIdsCommaSeparated: "113", contentOfPack: 2, existingScenarios: scenarios); // OR
            SaveScenario("A Circular Solution", 110,  contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("The Shackles Loosen", 111,  contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("The Bottom of It", 112,  contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("The Lost Thread", 113,  contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("Ink Not Yet Dry", 114,  unlockedScenarioIdsCommaSeparated: "115", contentOfPack: 2, existingScenarios: scenarios);
            SaveScenario("Future Uncertain", 115,  contentOfPack: 2, existingScenarios: scenarios);
        }

        internal static void SaveScenario(string name, int scenarionumber,
                                        string unlockedScenarioIdsCommaSeparated = "",
                                        string requiredGlobalAchievements = "",
                                        string requiredPartyAchievements = "",
                                        string blockingGlobalAchievements = "",
                                        string blockingPartyAchievements = "",
                                        int contentOfPack = 1, List<DL_Scenario> existingScenarios = null)
        {
            if (existingScenarios != null && existingScenarios.Any(x => x.Name == name)) return;

            var scenario = new DL_Scenario
            {
                Name = name,
                Scenarionumber = scenarionumber,
                UnlockedScenarios = unlockedScenarioIdsCommaSeparated,
                RequiredGlobalAchievements = requiredGlobalAchievements,
                RequiredPartyAchievements = requiredPartyAchievements,
                BlockingGlobalAchievements = blockingGlobalAchievements,
                BlockingPartyAchievements = blockingPartyAchievements,
                ContentOfPack = contentOfPack
            };

            ScenarioRepository.InsertOrReplace(scenario);
        }

        private static void AddFCGlobalAchievements()
        {
            Connection.BeginTransaction();
            try
            {
                SaveFCGlobalAchievements();
                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        internal static void SaveFCGlobalAchievements()
        {
            var aments = AchievementTypeRepository.Get();
            
            SaveAchievementType("Through the Portal", 17, contentOfPack: 2, existingAchievements:aments);
            SaveAchievementType("Knowledge is Power", 180, steps: 4, contentOfPack: 2, existingAchievements: aments);
            SaveAchievementType("Pieces of an Artifact", 19, steps: 3, contentOfPack: 2, existingAchievements: aments);
            SaveAchievementType("A Peril Averted", 2000, steps: 4, contentOfPack: 2, existingAchievements: aments);
            SaveAchievementType("Mechanical Splendor", 21, contentOfPack: 2, existingAchievements: aments);
            SaveAchievementType("Severed Ties", 22, contentOfPack: 2, existingAchievements: aments);
        }

        internal static void SaveAchievementType(string name, int internalNumber, int steps = 1, List<DL_Achievement> achievements = null, int contentOfPack = 1, List<DL_AchievementType> existingAchievements = null)
        {
            if (existingAchievements != null && existingAchievements.Any(x => x.Name == name)) return;

            if (achievements == null)
            {
                achievements = new List<DL_Achievement>();
            }

            var achievement = new DL_AchievementType
            {
                Name = name,
                InternalNumber = internalNumber,
                Steps = steps,
                Achievements = achievements,
                ContentOfPack = contentOfPack
            };

            AchievementTypeRepository.InsertOrReplace(achievement);
        }

        private static void FixSawHealPerk()
        {
            Connection.BeginTransaction();
            try
            {
                DL_ClassPerk perk = ClassPerkRepository.GetClassPerks(14).FirstOrDefault(x => x.Checkboxnumber == 13);

                if (perk != null)
                {
                    perk.Perktext = "Add one [RM] HEAL [H] 3, SELF card";
                    ClassPerkRepository.UpdatePerkText(perk);
                }

                perk = ClassPerkRepository.GetClassPerks(14).FirstOrDefault(x => x.Checkboxnumber == 14);
                if (perk != null)
                {
                    perk.Perktext = "Add one [RM] HEAL [H] 3, SELF card";

                    ClassPerkRepository.UpdatePerkText(perk);
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void AddClasses()
        {
            var classes = ClassRepository.Get();          

            Connection.BeginTransaction();
            try
            {
                if (!classes.Any(x=>x.ClassId == 0))
                {
                    // add Brute
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 0,
                        ClassName = "Inox Brute",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 1))
                {
                    // add Tinkerer
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 1,
                        ClassName = "Quatryl Tinkerer",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 2))
                {
                    // add Orchid Spellweaver
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 2,
                        ClassName = "Orchid Spellweaver",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 3))
                {
                    // add Human Scoundrel
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 3,
                        ClassName = "Human Scoundrel",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 4))
                {
                    // add Savvas Cragheart
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 4,
                        ClassName = "Savvas Cragheart",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 5))
                {
                    // add Vermling Mindthief
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 5,
                        ClassName = "Vermling Mindthief",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 6))
                {
                    // add Valrath Sunkeeper
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 6,
                        ClassName = "Valrath Sunkeeper",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 7))
                {
                    // add Valrath Quatermaster
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 7,
                        ClassName = "Valrath Quatermaster",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 8))
                {
                    // add Aesther Summoner
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 8,
                        ClassName = "Aesther Summoner",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 9))
                {
                    // add Aesther Nightshround
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 9,
                        ClassName = "Aesther Nightshround",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 10))
                {
                    // add Harrower Plagueherald
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 10,
                        ClassName = "Harrower Plagueherald",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }


                if (!classes.Any(x => x.ClassId == 11))
                {
                    // add Inox Berserker
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 11,
                        ClassName = "Inox Berserker",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 12))
                {
                    // add Quatryl Soothsinger
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 12,
                        ClassName = "Quatryl Soothsinger",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 13))
                {
                    // add Orchid Doomstalker
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 13,
                        ClassName = "Orchid Doomstalker",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 14))
                {
                    // add Human Sawbones
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 14,
                        ClassName = "Human Sawbones",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 15))
                {
                    // add Vermling Beasttyrant
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 15,
                        ClassName = "Vermling Beasttyrant",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 16))
                {
                    // add Vermling Beasttyrant
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 16,
                        ClassName = "Vermling Beasttyrant",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 17))
                {
                    // add Harrower Bladeswarm
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 17,
                        ClassName = "Harrower Bladeswarm",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 18))
                {
                    // add Aesther Diviner
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 18,
                        ClassName = "Aesther Diviner",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void SaveClass(DL_Class item)
        {
            ClassRepository.InsertOrReplace(item);
        }

        private static void AddDivinerClass()
        {
            Connection.BeginTransaction();
            try
            {
                var classes = ClassRepository.Get();

                if (!classes.Any(x => x.ClassId == 18))
                {
                    // add Aesther Diviner
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 18,
                        ClassName = "Aesther Diviner",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void AddItem151()
        {
            var items = ItemRepository.Get();
            SaveItem(itemnumber: 151, itemname: "Blade of the Sands", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
        }

        internal static void SaveItem(int itemnumber, string itemname, int itemcategorie, int itemcount, string itemtext, int itemprice, int prosperitylevel, List<DL_Item> items, int contentpack = 1)
        {
            if (items.Any(x => x.Itemnumber == itemnumber)) return;

            var item = new DL_Item
            {
                Itemcategorie = itemcategorie,
                Itemcount = itemcount,
                Itemname = itemname,
                Itemnumber = itemnumber,
                Itemprice = itemprice,
                Itemtext = itemtext,
                Prosperitylevel = prosperitylevel,
                ContentOfPack = contentpack
            };

            ItemRepository.InsertOrReplace(item);

            items.Add(item);
        }

        private static void FixNameOfScenario83()
        {
            var scenario = ScenarioRepository.Get(true).FirstOrDefault(x => x.Scenarionumber == 83);
            if (scenario == null) return;

            Connection.BeginTransaction();
            try
            {
                scenario.Name = "Shadows Within";

                ScenarioRepository.InsertOrReplace(scenario);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void FixQuatermasterStunPerk()
        {
            DL_ClassPerk perk = ClassPerkRepository.GetClassPerks(7).FirstOrDefault(x => x.Checkboxnumber == 10);

            if (perk != null)
            {
                perk.Perktext = "Add one [RM] STUN [ST] card";

                ClassPerkRepository.UpdatePerkText(perk);
            }
        }

        private static void FixElementalistStunPerk()
        {
            DL_ClassPerk perk = ClassPerkRepository.GetClassPerks(15).FirstOrDefault(x => x.Checkboxnumber == 14);

            if (perk != null)
            {
                perk.Perktext = "Add one [+0] STUN [ST] card";

                ClassPerkRepository.UpdatePerkText(perk);
            }
        }

        private static void FixScenario39unlockedScenarios()
        {
            ScenarioRepository.UpdateScenarioUnlockedScenarios(39, "46,15");

            var s15 = ScenarioRepository.GetScenarioByScenarioNumber(15);

            var campaignIDs = CampaignUnlockedScenarioRepository.CompletedScenario39AndScenario15NotUnlocked();

            var campaignsflat = CampaignRepository.GetCampaignsFlat();

            foreach (var camapignId in campaignIDs)
            {
                if (campaignsflat.Any(x=>x.Id == camapignId.ID_Campaign))
                {
                    try
                    {
                        var campaign = CampaignRepository.Get(camapignId.ID_Campaign);
                        var newScenario = new DL_CampaignUnlockedScenario()
                        {
                            Completed = false,
                            Campaign = campaign,
                            ID_Campaign = camapignId.ID_Campaign,
                            ID_Scenario = s15.Id,
                            Scenario = s15,
                            ScenarioTreasures = new List<DL_Treasure>()
                        };

                        CampaignUnlockedScenarioRepository.InsertOrReplace(newScenario);
                    }
                    catch(Exception ex)
                    {
                        // do nothing
                    }                   
                }               
            }
        }

        public static void CheckIfAllPerksExists()
        {
            List<DL_ClassPerk> perks = ClassPerkRepository.Get(false);

            AddBrutePerks(perks); // 0
            AddTinkererPerks(perks); // 1
            AddSpellweaverPerks(perks); // 2
            AddScoundrelPerks(perks); // 3
            AddCragheartPerks(perks); // 4
            AddMindthiefPerks(perks); // 5   

            AddSunPerks(perks); // 6
            AddQuatermasterPerks(perks); // 7
            AddSummonerPerks(perks); // 8
            AddNightShroudPerks(perks); // 9
            AddPlagueheraldPerks(perks); // 10
            AddLightningPerks(perks); // 11
            AddSoothsingerPerks(perks); // 12
            AddDoomstalkerPerks(perks); // 13
            AddSawbonesPerks(perks); // 14
            AddElementalistPerks(perks); // 15
            AddBeastTyrantPerks(perks); // 16   

            AddBladeswarmPerks(perks); // 17
            AddDivinerClassPerks(perks); // 18

            //perks = ClassPerkRepository.Get(false);

            //MigrateSunPerks(perks);
            //MigrateQuatermasterPerks(perks);
            //MigrateSummonerPerks(perks);
            //MigrateNightShroudPerks(perks);
            //MigratePlagueheraldPerks(perks);
            //MigrateLightningPerks(perks);
            //MigrateSoothsingerPerks(perks);
            //MigrateDoomstalkerPerks(perks);
            //MigrateSawbonePerks(perks);
            //MigrateElementalistPerks(perks);
            //MigrateBeastTyrantPerks(perks);
        }

        private static void AddDivinerClassPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 18))
            {
                Connection.BeginTransaction();
                try
                {
                    GloomhavenDbHelper.InsertClassPerk(18, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(18, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(18, "Remove one [-2] cards", 3);
                    GloomhavenDbHelper.InsertClassPerk(18, "Replace two [+1] card with one [+3] SHIELD [SH] 1, Self card", 4);
                    GloomhavenDbHelper.InsertClassPerk(18, "Replace two [+1] card with one [+3] SHIELD [SH] 1, Self card", 5);
                    GloomhavenDbHelper.InsertClassPerk(18, "Replace one [+0] card with one [+1] SHIELD [SH] 1, Affect any ally card", 6);
                    GloomhavenDbHelper.InsertClassPerk(18, "Replace one [+0] card with one [+2] [DARK] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(18, "Replace one [+0] card with one [+2] [LIGHT] card", 8);
                    GloomhavenDbHelper.InsertClassPerk(18, "Replace one [+0] card with one [+3] MUDDLE [M] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(18, "Replace one [+0] card with one [+2] CURSE [C] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(18, "Replace one [+0] card with one [+2] REGENERATE [RE], Self card", 11);
                    GloomhavenDbHelper.InsertClassPerk(18, "Replace one [-1] card with one [+1] Heal [H] 2, Affect any ally card", 12);
                    GloomhavenDbHelper.InsertClassPerk(18, "Add two [RM] Heal [H] 1, Self cards", 13);
                    GloomhavenDbHelper.InsertClassPerk(18, "Add two [RM] CURSE [C] cards", 14);
                    GloomhavenDbHelper.InsertClassPerk(18, "Ignore negative scenario effects and add two [+1] cards", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddBladeswarmPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 17))
            {
                Connection.BeginTransaction();
                try
                {
                    GloomhavenDbHelper.InsertClassPerk(17, "Remove one [-2] card", 1);
                    GloomhavenDbHelper.InsertClassPerk(17, "Remove four [+0] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(17, "Replace one [-1] card with one [+1][WIND] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(17, "Replace one [-1] card with one [+1][EARTH] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(17, "Replace one [-1] card with one [+1][LIGHT] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(17, "Replace one [-1] card with one [+1][DARK] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(17, "Add two [RM] Heal [H]1 cards", 7);
                    GloomhavenDbHelper.InsertClassPerk(17, "Add two [RM] Heal [H]1 cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(17, "Add one [+1] WOUND [W] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(17, "Add one [+1] WOUND [W] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(17, "Add one [+1] POISON [PO] card", 11);
                    GloomhavenDbHelper.InsertClassPerk(17, "Add one [+1] POISON [PO] card", 12);
                    GloomhavenDbHelper.InsertClassPerk(17, "Add one [+2] MUDDLE [M] card", 13);
                    GloomhavenDbHelper.InsertClassPerk(17, "Ignore negative item effects and add one [+1] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(17, "Ignore negative scenario effects and add one [+1] card", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddScoundrelPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 3))
            {
                Connection.BeginTransaction();
                try
                {
                    GloomhavenDbHelper.InsertClassPerk(3, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(3, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(3, "Remove four [+0] cards", 3);
                    GloomhavenDbHelper.InsertClassPerk(3, "Replace one [-2] card with one [+0] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(3, "Replace one [-1] card with one [+1] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(3, "Replace one [+0] card with one [+2] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(3, "Replace one [+0] card with one [+2] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(3, "Add two [RM] [+1] cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(3, "Add two [RM] [+1] cards", 9);
                    GloomhavenDbHelper.InsertClassPerk(3, "Add two [RM] PIERCE [PI] 3 cards", 10);
                    GloomhavenDbHelper.InsertClassPerk(3, "Add two [RM] POISON [PO] cards", 11);
                    GloomhavenDbHelper.InsertClassPerk(3, "Add two [RM] POISON [PO] cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(3, "Add two [RM] MUDDLE [M] cards", 13);
                    GloomhavenDbHelper.InsertClassPerk(3, "Add one [RM] INVISIBLE [IN] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(3, "Ignore negative scenario effects", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddSpellweaverPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 2))
            {
                Connection.BeginTransaction();
                try
                {
                    GloomhavenDbHelper.InsertClassPerk(2, "Remove four [+0] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(2, "Replace one [-1] card with one [+1] card", 2);
                    GloomhavenDbHelper.InsertClassPerk(2, "Replace one [-1] card with one [+1] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add two [+1] cards", 4);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add two [+1] cards", 5);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [+0] STUN [ST] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [+1] WOUND [W] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [+1] IMMOBILIZE [I] card", 8);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [+1] CURSE [C] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [+2] [FIRE] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [+2] [FIRE] card", 11);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [+2] [FROST] card", 12);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [+2] [FROST] card", 13);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [RM] [EARTH] and one [RM] [WIND] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(2, "Add one [RM] [LIGHT] and one [RM] [DARK] card", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddMindthiefPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 5))
            {
                Connection.BeginTransaction();
                try
                {
                    GloomhavenDbHelper.InsertClassPerk(5, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(5, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(5, "Remove four [+0] cards", 3);
                    GloomhavenDbHelper.InsertClassPerk(5, "Replace two [+1] cards with two [+2] cards", 4);
                    GloomhavenDbHelper.InsertClassPerk(5, "Replace one [-2] card with one [+0] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(5, "Add one [+2] [FROST] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(5, "Add one [+2] [FROST] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(5, "Add two [RM] [+1] cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(5, "Add two [RM] [+1] cards", 9);
                    GloomhavenDbHelper.InsertClassPerk(5, "Add three [RM] PULL [PUL] 1 cards", 10);
                    GloomhavenDbHelper.InsertClassPerk(5, "Add three [RM] MUDDLE [M] cards", 11);
                    GloomhavenDbHelper.InsertClassPerk(5, "Add two [RM] IMMOBILIZE [I] cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(5, "Add one [RM] STUN [ST] card", 13);
                    GloomhavenDbHelper.InsertClassPerk(5, "Add one [RM] DISARM [D] card and one [RM] MUDDLE [M] card ", 14);
                    GloomhavenDbHelper.InsertClassPerk(5, "Ignore negative scenario effects", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddCragheartPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 4))
            {
                Connection.BeginTransaction();
                try
                {
                    GloomhavenDbHelper.InsertClassPerk(4, "Remove four [+0] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(4, "Replace one [-1] card with one [+1] card", 2);
                    GloomhavenDbHelper.InsertClassPerk(4, "Replace one [-1] card with one [+1] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(4, "Replace one [-1] card with one [+1] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(4, "Add one [-2] card and two [+2] cards", 5);
                    GloomhavenDbHelper.InsertClassPerk(4, "Add one [+1] IMMOBILIZE [I] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(4, "Add one [+1] IMMOBILIZE [I] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(4, "Add one [+2] MUDDLE [M] card", 8);
                    GloomhavenDbHelper.InsertClassPerk(4, "Add one [+2] MUDDLE [M] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(4, "Add two [RM] PUSH [PU] 2 cards", 10);
                    GloomhavenDbHelper.InsertClassPerk(4, "Add two [RM] [EARTH] cards", 11);
                    GloomhavenDbHelper.InsertClassPerk(4, "Add two [RM] [EARTH] cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(4, "Add two [RM] [WIND] cards", 13);
                    GloomhavenDbHelper.InsertClassPerk(4, "Ignore negative item effects", 14);
                    GloomhavenDbHelper.InsertClassPerk(4, "Ignore negative scenario effects", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddTinkererPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 1))
            {
                Connection.BeginTransaction();
                try
                {
                    GloomhavenDbHelper.InsertClassPerk(1, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(1, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(1, "Replace one [-2] card with one [+0] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add two [+1] cards", 4);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add one [+3] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add two [RM] [FIRE] cards", 6);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add three [RM] MUDDLE [M] cards", 7);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add one [+1] WOUND [W] card", 8);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add one [+1] WOUND [W] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add one [+1] IMMOBILIZE [I] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add one [+1] IMMOBILIZE [I] card", 11);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add one [+1] Heal [H]2, self card", 12);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add one [+1] Heal [H]2, self card", 13);
                    GloomhavenDbHelper.InsertClassPerk(1, "Add one [+0] ADD TARGET [T] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(1, "Ignore negative scenario effects", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddBrutePerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 0))
            {
                Connection.BeginTransaction();
                try
                {
                    GloomhavenDbHelper.InsertClassPerk(0, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(0, "Replace one [-1] card with one [+1] card", 2);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add two [+1] cards", 3);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add two [+1] cards", 4);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add one [+3] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add three [RM] PUSH [PU] 1 cards", 6);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add three [RM] PUSH [PU] 1 cards", 7);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add two [RM] PIERCE [PI] 3 cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add one [RM] STUN [ST] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add one [RM] STUN [ST] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add one [RM] DISARM [D] card and one [RM] MUDDLE [M] card", 11);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add one [RM] ADD TARGET [T] card", 12);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add one [RM] ADD TARGET [T] card", 13);
                    GloomhavenDbHelper.InsertClassPerk(0, "Add one [+1] Shield [SH] 1, Self card", 14);
                    GloomhavenDbHelper.InsertClassPerk(0, "Ignore negative item effects and add one [+1] card", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void FixScenarioRegionOf61()
        {
            Connection.BeginTransaction();
            try
            {
                var sdc = new ScenarioDataService();
                sdc.UpdateScenarioRegion(61, 4);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void FixScenarioUnlockedScenariosOf39()
        {
            Connection.BeginTransaction();
            try
            {
                var sdc = new ScenarioDataService();
                sdc.UpdateUnlockedScenarios(39, "46,15");

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void FixNameOfGA130()
        {
            var iten = AchievementTypeRepository.Get(false).FirstOrDefault(x => x.InternalNumber == 130);
            if (iten == null) return;

            Connection.BeginTransaction();
            try
            {
                iten.Name = "End of Corruption";

                AchievementTypeRepository.InsertOrReplace(iten);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void FixNameOfPQ533()
        {
            var pq = PersonalQuestRepository.Get(true).FirstOrDefault(x => x.QuestNumber == 533);
            if (pq == null) return;

            Connection.BeginTransaction();
            try
            {
                pq.QuestName = "The Perfect Poison";
                PersonalQuestRepository.InsertOrReplace(pq);
                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void FixNameOfScenario54()
        {
            var scenario = ScenarioRepository.Get(true).FirstOrDefault(x => x.Scenarionumber == 54);
            if (scenario == null) return;

            Connection.BeginTransaction();
            try
            {
                scenario.Name = "Palace of Ice";

                ScenarioRepository.InsertOrReplace(scenario);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void FixNameOfScenario31()
        {
            var scenario = ScenarioRepository.Get(true).FirstOrDefault(x => x.Scenarionumber == 31);
            if (scenario == null) return;

            Connection.BeginTransaction();
            try
            {
                scenario.Name = "Plane of Night";

                ScenarioRepository.InsertOrReplace(scenario);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void UpdatePersonalQuestCountersWithScenarios()
        {
            if (PersonalQuestCountersRepository.Get(false).Any())
            {
                var counters = new List<DL_PersonalQuestCounter>();
                var pqs = PersonalQuestRepository.GetWithNumbers("510, 513, 521, 526, 529", false);

                Connection.BeginTransaction();
                try
                {
                    foreach (var pq in pqs)
                    {
                        DL_PersonalQuestCounter newCounter = null;
                        switch (pq.QuestNumber)
                        {
                            case 510:
                                newCounter = GloomhavenDbHelper.InsertPQCounter(pq, "Completed quest starting with scenario #52", 1, 0);
                                break;
                            case 513:
                                newCounter = GloomhavenDbHelper.InsertPQCounter(pq, "Completed quest starting with scenario #59", 1, 0);
                                break;
                            case 521:
                                newCounter = GloomhavenDbHelper.InsertPQCounter(pq, "Completed quest starting with scenario #55", 1, 0);
                                break;
                            case 526:
                                newCounter = GloomhavenDbHelper.InsertPQCounter(pq, "Completed quest starting with scenario #57", 1, 0);
                                break;
                            case 529:
                                newCounter = GloomhavenDbHelper.InsertPQCounter(pq, "Completed quest starting with scenario #61", 1, 0);
                                break;
                        }

                        if (newCounter != null)
                        {
                            counters.Add(newCounter);
                        }
                    }

                    Connection.Commit();
                }
                catch (Exception ex)
                {
                    Connection.Rollback();
                    throw ex;
                }

                Connection.BeginTransaction();
                try
                {
                    foreach (DL_PersonalQuestCounter counter in counters)
                    {
                        foreach (var c in CharacterRepository.Get(false).Where(x => x.ID_PersonalQuest == counter.PersonalQuest_ID))
                        {
                            AddCharacterPQCounter(counter, c);
                        }
                    }

                    Connection.Commit();
                }
                catch (Exception ex)
                {
                    Connection.Rollback();
                    throw ex;
                }
            }
        }

        private static void UpdatePersonalQuestCountersFor520()
        {
            var pq520 = PersonalQuestCountersRepository.Get(true).FirstOrDefault(x => x.PersonalQuest.QuestNumber == 520);

            if (pq520 != null)
            {
                Connection.BeginTransaction();

                try
                {
                    pq520.CounterName = "Killed 'Living' Monsters with Skullbane Axe";
                    PersonalQuestCountersRepository.InsertOrReplace(pq520);

                    Connection.Commit();
                }
                catch (Exception ex)
                {
                    Connection.Rollback();
                    throw ex;
                }
            }
        }

        private static void FillCharacterPersonalQuestCounters()
        {
            if (!CharacterPersonalQuestCountersRepository.Get(false).Any())
            {
                Connection.BeginTransaction();
                try
                {
                    var pqCounters = PersonalQuestCountersRepository.Get(false);
                    foreach (var c in CharacterRepository.Get(false))
                    {
                        if (c.ID_PersonalQuest > 0)
                        {
                            foreach (var counter in pqCounters.Where(x => x.PersonalQuest_ID == c.ID_PersonalQuest))
                            {
                                AddCharacterPQCounter(counter, c);
                            }
                        }
                    }
                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddCharacterPQCounter(DL_PersonalQuestCounter counter, DL_Character c)
        {
            var item = new DL_CharacterPersonalQuestCounter
            {
                Character_Id = c.Id,
                PersonalQuestCounter_Id = counter.Id,
                Character = c,
                PersonalQuestCounter = counter,
                Value = 0
            };

            CharacterPersonalQuestCountersRepository.InsertOrReplace(item);
        }

        private static void MigrateUnlockedItems()
        {
            Connection.BeginTransaction();
            try
            {
                var items = ItemRepository.Get();
                var campaigns = CampaignRepository.Get();
                foreach (DL_Campaign c in campaigns)
                {
                    if (c?.CampaignUnlocks?.UnlockedItemDesignNumbers.Length > 0 && items.Any())
                    {
                        foreach (int unlockedItem in Helper.StringToIntList(c.CampaignUnlocks.UnlockedItemDesignNumbers))
                        {
                            var item = items.FirstOrDefault(x => x.Itemnumber == unlockedItem);

                            if (item != null)
                            {
                                DL_CampaignUnlockedItem ui = new DL_CampaignUnlockedItem()
                                {
                                    Campaign = c,
                                    ID_Campaign = c.Id,
                                    ID_Item = item.Id,
                                    Item = item
                                };

                                CampaignUnlockedItemRepository.InsertOrReplace(ui);
                            }
                        }
                    }

                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigratePersonalQuests()
        {
            Connection.BeginTransaction();
            try
            {
                var pqs = PersonalQuestRepository.Get();
                foreach (DL_Character c in CharacterRepository.Get())
                {
                    var pq = pqs.FirstOrDefault(x => x.QuestNumber == c.LifegoalNumber);
                    if (pq != null && c.PersonalQuest == null)
                    {
                        c.ID_PersonalQuest = pq.Id;
                        c.PersonalQuest = pq;
                    }

                    CharacterRepository.InsertOrReplace(c);
                }
                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigrateAbillities()
        {
            Connection.BeginTransaction();
            try
            {
                foreach (DL_Ability a in AbilityRepository.Get())
                {
                    if (int.TryParse(a.Name, out int aNumber))
                    {
                        a.ReferenceNumber = aNumber;
                    }

                    AbilityRepository.InsertOrReplace(a);
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void UpdateDBFrom_004_to_005()
        {
            var scenarios = ScenarioRepository.Get();
            var changedScenarios = new List<DL_Scenario>();

            // scenario 19: requires ga 8, ga 8 is not blocking (again, forgot to update init)
            var scen19 = scenarios.FirstOrDefault(x => x.Scenarionumber == 19);
            if (scen19 != null)
            {
                scen19.BlockingGlobalAchievements = "";
                scen19.RequiredGlobalAchievements = "8";
                changedScenarios.Add(scen19);
            }

            // scenario 8: unlocks scenario 13
            var scen8 = scenarios.FirstOrDefault(x => x.Scenarionumber == 8);
            if (scen8 != null)
            {
                scen8.UnlockedScenarios = "13,14,7";
                changedScenarios.Add(scen8);
            }

            // scenario 41 does not unlock scenario 42
            var scen41 = scenarios.FirstOrDefault(x => x.Scenarionumber == 41);
            if (scen41 != null)
            {
                scen41.UnlockedScenarios = "";
                changedScenarios.Add(scen41);
            }

            // scenario 42 does not unlock scenario 41
            var scen42 = scenarios.FirstOrDefault(x => x.Scenarionumber == 42);
            if (scen42 != null)
            {
                scen42.UnlockedScenarios = "";
                changedScenarios.Add(scen42);
            }

            // wrong string
            var scen34 = scenarios.FirstOrDefault(x => x.Scenarionumber == 34);
            if (scen34 != null)
            {
                scen34.RequiredPartyAchievements = "17";
                changedScenarios.Add(scen34);
            }

            // wrong string
            var scen35 = scenarios.FirstOrDefault(x => x.Scenarionumber == 35);
            if (scen35 != null)
            {
                scen35.RequiredPartyAchievements = "1";
                changedScenarios.Add(scen35);
            }

            // wrong string
            var scen36 = scenarios.FirstOrDefault(x => x.Scenarionumber == 36);
            if (scen36 != null)
            {
                scen36.RequiredPartyAchievements = "1";
                changedScenarios.Add(scen36);
            }

            ScenarioRepository.InsertOrReplace(changedScenarios);

            var campaignScenarios = CampaignUnlockedScenarioRepository.Get();

            // Unlock scenario 13 for campaigns where scenario 8 is completed
            var campScenario8 = campaignScenarios.FirstOrDefault(x => x.Scenario.Scenarionumber == 8 && x.Completed && x.Campaign != null);

            if (campScenario8 == null) return;
            if (campaignScenarios.FirstOrDefault(x => x.Scenario.Scenarionumber == 13) != null) return;

            var scen13 = scenarios.FirstOrDefault(x => x.Scenarionumber == 13);
            if (scen13 == null) return;

            var unlockedScenarioData = new DL_CampaignUnlockedScenario
            {
                Scenario = scen13,
                Campaign = campScenario8.Campaign,
                ID_Scenario = scen13.Id,
                ID_Campaign = campScenario8.ID_Campaign,
                Completed = false,
                ScenarioTreasures = new List<DL_Treasure>()
            };

            CampaignUnlockedScenarioRepository.InsertOrReplace(unlockedScenarioData);
        }

        private static void FixNameOfScenario22()
        {
            var scenario = ScenarioRepository.Get(false).FirstOrDefault(x => x.Scenarionumber == 22);
            if (scenario == null) return;

            Connection.BeginTransaction();
            try
            {
                scenario.Name = "Temple of the Elements";

                ScenarioRepository.InsertOrReplace(scenario);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void FixTinkererHealPerk(List<DL_ClassPerk> perks)
        {
            var tinkererHealPerks = perks.Where(x => x.ClassId == 1 && (x.Checkboxnumber == 12 || x.Checkboxnumber == 13));

            Connection.BeginTransaction();
            try
            {
                foreach (var p in tinkererHealPerks)
                {
                    p.Perktext = "Add one [+1] Heal [H]2, self card";
                    ClassPerkRepository.InsertOrReplace(p);
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigrateSoothsingerPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 12);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 13);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigrateQuatermasterPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 7);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 8);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigratePlagueheraldPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 10);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 11);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigrateNightShroudPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 9);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 10);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigrateElementalistPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 15);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 16);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigrateBeastTyrantPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 16);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 17);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void AddSoothsingerPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 12))
            {
                Connection.BeginTransaction();
                try
                {
                    // Soothsinger
                    GloomhavenDbHelper.InsertClassPerk(12, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(12, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(12, "Remove one [-2] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(12, "Replace two [+1] cards with one [+4] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(12, "Replace two [+1] cards with one [+4] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(12, "Replace one [+0] card with one [+1] IMMOBILIZE [I] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(12, "Replace one [+0] card with one [+1] DISARM [D] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(12, "Replace one [+0] card with one [+2] WOUND [W] card", 8);
                    GloomhavenDbHelper.InsertClassPerk(12, "Replace one [+0] card with one [+2] POISON [PO] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(12, "Replace one [+0] card with one [+2] CURSE [C] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(12, "Replace one [+0] card with one [+3] MUDDLE [M] card", 11);
                    GloomhavenDbHelper.InsertClassPerk(12, "Replace one [-1] card with one [+0] STUN [ST] card", 12);
                    GloomhavenDbHelper.InsertClassPerk(12, "Add three [RM] [+1] cards", 13);
                    GloomhavenDbHelper.InsertClassPerk(12, "Add two [RM] CURSE [C] cards", 14);
                    GloomhavenDbHelper.InsertClassPerk(12, "Add two [RM] CURSE [C] cards", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddQuatermasterPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 7))
            {
                Connection.BeginTransaction();
                try
                {
                    // Quatermaster
                    GloomhavenDbHelper.InsertClassPerk(7, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(7, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(7, "Remove four [+0] cards", 3);
                    GloomhavenDbHelper.InsertClassPerk(7, "Replace one [+0] card with one [+2] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(7, "Replace one [+0] card with one [+2] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(7, "Add two [RM] [+1] cards", 6);
                    GloomhavenDbHelper.InsertClassPerk(7, "Add two [RM] [+1] cards", 7);
                    GloomhavenDbHelper.InsertClassPerk(7, "Add three [RM] MUDDLE [M] cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(7, "Add two [RM] PIERCE [PI] 3 cards", 9);
                    GloomhavenDbHelper.InsertClassPerk(7, "Add one [RM] STUN [ST] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(7, "Add one [RM] ADD TARGET [T] card", 11);
                    GloomhavenDbHelper.InsertClassPerk(7, "Add one [+0] Refresh an item card", 12);
                    GloomhavenDbHelper.InsertClassPerk(7, "Add one [+0] Refresh an item card", 13);
                    GloomhavenDbHelper.InsertClassPerk(7, "Add one [+0] Refresh an item card", 14);
                    GloomhavenDbHelper.InsertClassPerk(7, "Ignore negative item effects and add two [+1] cards", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddPlagueheraldPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 10))
            {
                Connection.BeginTransaction();
                try
                {
                    // Plagueherald
                    GloomhavenDbHelper.InsertClassPerk(10, "Replace one [-2] card with one [+0] card", 1);
                    GloomhavenDbHelper.InsertClassPerk(10, "Replace one [-1] card with one [+1] card", 2);
                    GloomhavenDbHelper.InsertClassPerk(10, "Replace one [-1] card with one [+1] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(10, "Replace one [+0] card with one [+2] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(10, "Replace one [+0] card with one [+2] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(10, "Add two [+1] cards", 6);
                    GloomhavenDbHelper.InsertClassPerk(10, "Add one [+1] [WIND] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(10, "Add one [+1] [WIND] card", 8);
                    GloomhavenDbHelper.InsertClassPerk(10, "Add one [+1] [WIND] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(10, "Add three [RM] POISON [PO] cards", 10);
                    GloomhavenDbHelper.InsertClassPerk(10, "Add two [RM] CURSE [C] cards", 11);
                    GloomhavenDbHelper.InsertClassPerk(10, "Add two [RM] IMMOBILIZE [I] cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(10, "Add one [RM] STUN [ST] card", 13);
                    GloomhavenDbHelper.InsertClassPerk(10, "Add one [RM] STUN [ST] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(10, "Ignore negative scenario effects and add one [+1] card", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddNightShroudPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 9))
            {
                Connection.BeginTransaction();
                try
                {
                    // Nightshroud
                    GloomhavenDbHelper.InsertClassPerk(9, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(9, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(9, "Remove four [+0] cards", 3);
                    GloomhavenDbHelper.InsertClassPerk(9, "Add one [-1] [DARK] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(9, "Add one [-1] [DARK] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(9, "Replace one [-1] [DARK] card with one [+1] [DARK] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(9, "Replace one [-1] [DARK] card with one [+1] [DARK] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(9, "Add one [+1] INVISIBLE [IN] cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(9, "Add one [+1] INVISIBLE [IN] cards", 9);
                    GloomhavenDbHelper.InsertClassPerk(9, "Add three [RM] MUDDLE [M] cards", 10);
                    GloomhavenDbHelper.InsertClassPerk(9, "Add three [RM] MUDDLE [M] cards", 11);
                    GloomhavenDbHelper.InsertClassPerk(9, "Add two [RM] HEAL [H] 1, Self cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(9, "Add two [RM] CURSE [C] cards", 13);
                    GloomhavenDbHelper.InsertClassPerk(9, "Add one [RM] ADD TARGET [T] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(9, "Ignore negative scenario effects and add two [+1] cards", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddElementalistPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 15))
            {
                Connection.BeginTransaction();
                try
                {
                    // Elementalist
                    GloomhavenDbHelper.InsertClassPerk(15, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(15, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(15, "Replace one [-1] card with one [+1] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(15, "Replace one [+0] card with one [+2] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(15, "Replace one [+0] card with one [+2] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(15, "Add three [+0] [FIRE] cards", 6);
                    GloomhavenDbHelper.InsertClassPerk(15, "Add three [+0] [FROST] cards", 7);
                    GloomhavenDbHelper.InsertClassPerk(15, "Add three [+0] [WIND] cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(15, "Add three [+0] [EARTH] cards", 9);
                    GloomhavenDbHelper.InsertClassPerk(15, "Replace two [+0] cards with one [+0] [FIRE] and one [+0] [EARTH] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(15, "Replace two [+0] cards with one [+0] [FROST] and one [+0] [WIND] card", 11);
                    GloomhavenDbHelper.InsertClassPerk(15, "Add two [+1] PUSH [PU] 1 cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(15, "Add one [+1] WOUND [W] card", 13);
                    GloomhavenDbHelper.InsertClassPerk(15, "Add one [+0] STUN [ST] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(15, "Add one [+0] ADD TARGET [T] card", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddBeastTyrantPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 16))
            {
                Connection.BeginTransaction();
                try
                {
                    // BeastTyrant
                    GloomhavenDbHelper.InsertClassPerk(16, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(16, "Replace one [-1] card with one [+1] card", 2);
                    GloomhavenDbHelper.InsertClassPerk(16, "Replace one [-1] card with one [+1] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(16, "Replace one [-1] card with one [+1] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(16, "Replace one [+0] card with one [+2] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(16, "Replace one [+0] card with one [+2] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(16, "Add one [+1] WOUND [W] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(16, "Add one [+1] WOUND [W] card", 8);
                    GloomhavenDbHelper.InsertClassPerk(16, "Add one [+1] IMMOBILIZE [I] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(16, "Add one [+1] IMMOBILIZE [I] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(16, "Add two [RM] HEAL [H] 1, Self cards", 11);
                    GloomhavenDbHelper.InsertClassPerk(16, "Add two [RM] HEAL [H] 1, Self cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(16, "Add two [RM] HEAL [H] 1, Self cards", 13);
                    GloomhavenDbHelper.InsertClassPerk(16, "Add two [RM] [EARTH] cards", 14);
                    GloomhavenDbHelper.InsertClassPerk(16, "Ignore negative scenario effects", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddPartyAchievementSunblessed()
        {
            var partyachievements = PartyAchievementRepository.Get().Where(x => x.InternalNumber == 28 && x.Name == "Sun Blessed");

            if (!partyachievements.Any())
            {
                Connection.BeginTransaction();
                try
                {
                    SavePartyAchievement(
                       28,
                       "Sun Blessed", contentOfPack:1
                    );

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void FixItem33ItemCategorie()
        {
            Connection.BeginTransaction();
            try
            {
                var item = ItemRepository.Get().FirstOrDefault(x => x.Itemnumber == 33);
                if (item != null && item.Itemcategorie == 5)
                {
                    item.Itemcategorie = 2;
                    ItemRepository.InsertOrReplace(item);
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigrateSummonerPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 8);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 9);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigrateDoomstalkerPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 13);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 14);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void AddDoomstalkerPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 13))
            {
                Connection.BeginTransaction();
                try
                {
                    // Doomstalker
                    GloomhavenDbHelper.InsertClassPerk(13, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(13, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(13, "Replace two [+0] cards with two [+1] cards", 3);
                    GloomhavenDbHelper.InsertClassPerk(13, "Replace two [+0] cards with two [+1] cards", 4);
                    GloomhavenDbHelper.InsertClassPerk(13, "Replace two [+0] cards with two [+1] cards", 5);
                    GloomhavenDbHelper.InsertClassPerk(13, "Add two [RM] [+1] cards", 6);
                    GloomhavenDbHelper.InsertClassPerk(13, "Add two [RM] [+1] cards", 7);
                    GloomhavenDbHelper.InsertClassPerk(13, "Add one [+2] MUDDLE [M] card", 8);
                    GloomhavenDbHelper.InsertClassPerk(13, "Add one [+1] POISON [PO] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(13, "Add one [+1] WOUND [W] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(13, "Add one [+1] IMMOBILIZE [I] card", 11);
                    GloomhavenDbHelper.InsertClassPerk(13, "Add one [+0] STUN [ST] card", 12);
                    GloomhavenDbHelper.InsertClassPerk(13, "Add one [RM] ADD TARGET [T] card", 13);
                    GloomhavenDbHelper.InsertClassPerk(13, "Add one [RM] ADD TARGET [T] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(13, "Ignore negative scenario effects", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void AddSummonerPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 8))
            {
                Connection.BeginTransaction();
                try
                {
                    // Summoner
                    GloomhavenDbHelper.InsertClassPerk(8, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(8, "Replace one [-2] card with one [+0] card", 2);
                    GloomhavenDbHelper.InsertClassPerk(8, "Replace one [-1] card with one [+1] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(8, "Replace one [-1] card with one [+1] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(8, "Replace one [-1] card with one [+1] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(8, "Add one [+2] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(8, "Add one [+2] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(8, "Add two [RM] WOUND [W] cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(8, "Add two [RM] POISON [PO] cards", 9);
                    GloomhavenDbHelper.InsertClassPerk(8, "Add two [RM] HEAL [H] 1, Self cards", 10);
                    GloomhavenDbHelper.InsertClassPerk(8, "Add two [RM] HEAL [H] 1, Self cards", 11);
                    GloomhavenDbHelper.InsertClassPerk(8, "Add two [RM] HEAL [H] 1, Self cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(8, "Add one [RM] [FIRE] and one [RM] [WIND] card", 13);
                    GloomhavenDbHelper.InsertClassPerk(8, "Add one [RM] [DARK] and one [RM] [EARTH] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(8, "Ignore negative scenario effects and add two [+1] cards", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void MigrateSunPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 6);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 7);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigratePerks(IEnumerable<DL_ClassPerk> classperks, IEnumerable<DL_Character> characters)
        {
            foreach (DL_Character c in characters)
            {
                var chara = CharacterRepository.Get(c.Id);
                if (chara.Perks.Any())
                {
                    foreach (DL_Perk oldperk in chara.Perks.Where(x => x.Checkboxnumber > 0 && x.Checkboxnumber < 16))
                    {
                        if (!chara.CharacterPerks.Any(x => x.Checkboxnumber == oldperk.Checkboxnumber))
                        {
                            var newPerk = classperks.FirstOrDefault(x => x.ClassId == chara.ClassId - 1 && x.Checkboxnumber == oldperk.Checkboxnumber);

                            if (newPerk != null)
                            {
                                chara.CharacterPerks.Add(newPerk);
                                CharacterRepository.InsertOrReplace(chara);
                            }
                        }
                    }
                }
            }
        }

        private static void AddSunPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 6))
            {
                Connection.BeginTransaction();
                try
                {
                    // RM = Rolling Modifier
                    // PU = Push
                    // PI = Pierce
                    // ST = Stun
                    // D = Disarm
                    // I = Imobilize
                    // M = Muddle
                    // W = Wound
                    // T = Add Target
                    // SH = Shield
                    // H = Heal

                    // Sun
                    GloomhavenDbHelper.InsertClassPerk(6, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(6, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(6, "Remove four [+0] cards", 3);
                    GloomhavenDbHelper.InsertClassPerk(6, "Replace one [-2] card with one [+0] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(6, "Replace one [+0] card with one [+2] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(6, "Add two [RM] [+1] cards", 6);
                    GloomhavenDbHelper.InsertClassPerk(6, "Add two [RM] [+1] cards", 7);
                    GloomhavenDbHelper.InsertClassPerk(6, "Add two [RM] HEAL [H] 1, self cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(6, "Add two [RM] HEAL [H] 1, self cards", 9);
                    GloomhavenDbHelper.InsertClassPerk(6, "Add one [RM] STUN [ST] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(6, "Add two [RM] [LIGHT] cards", 11);
                    GloomhavenDbHelper.InsertClassPerk(6, "Add two [RM] [LIGHT] cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(6, "Add two [RM] Shield [SH] 1, self cards", 13);
                    GloomhavenDbHelper.InsertClassPerk(6, "Ignore negative item effects and add two [+1] cards", 14);
                    GloomhavenDbHelper.InsertClassPerk(6, "Ignore negative scenario effects", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void MigrateLightningPerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var classperks = perks.Where(x => x.ClassId == 11);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 12);
                MigratePerks(classperks, characters);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void AddLightningPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 11))
            {
                Connection.BeginTransaction();
                try
                {
                    // RM = Rolling Modifier
                    // PU = Push
                    // PI = Pierce
                    // ST = Stun
                    // D = Disarm
                    // I = Imobilize
                    // M = Muddle
                    // W = Wound
                    // T = Add Target
                    // SH = Shield
                    // H = Heal

                    // Lightning
                    GloomhavenDbHelper.InsertClassPerk(11, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(11, "Remove four [+0] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(11, "Replace one [-1] card with one [+1] card", 3);
                    GloomhavenDbHelper.InsertClassPerk(11, "Replace one [-1] card with one [+1] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(11, "Replace one [+0] card with one [RM] [+2] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(11, "Replace one [+0] card with one [RM] [+2] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(11, "Add two [RM] WOUND [W] cards", 7);
                    GloomhavenDbHelper.InsertClassPerk(11, "Add two [RM] WOUND [W] cards", 8);
                    GloomhavenDbHelper.InsertClassPerk(11, "Add one [RM] STUN [ST] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(11, "Add one [RM] STUN [ST] card", 10);
                    GloomhavenDbHelper.InsertClassPerk(11, "Add one [RM] [+1] DISARM [D] card", 11);
                    GloomhavenDbHelper.InsertClassPerk(11, "Add two [RM] HEAL [H] 1 SELF cards", 12);
                    GloomhavenDbHelper.InsertClassPerk(11, "Add one [+2] [FIRE] card", 13);
                    GloomhavenDbHelper.InsertClassPerk(11, "Add one [+2] [FIRE] card", 14);
                    GloomhavenDbHelper.InsertClassPerk(11, "Ignore negative item effects", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void FixScenarioRegions()
        {
            Connection.BeginTransaction();
            try
            {
                var sdc = new ScenarioDataService();
                sdc.UpdateScenarioRegion(79, 4);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void FixScoundrelPerkTypo()
        {
            var perks = ClassPerkRepository.Get(false);
            var scoundrelPerks = perks.Where(x => x.ClassId == 3 && (x.Checkboxnumber == 11 || x.Checkboxnumber == 12));

            Connection.BeginTransaction();
            try
            {
                foreach (var p in scoundrelPerks)
                {
                    p.Perktext = "Add two [RM] POISON [PO] cards";
                    ClassPerkRepository.InsertOrReplace(p);
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void FixMindthiefPerkTypo()
        {
            var perks = ClassPerkRepository.Get(false);
            var scoundrelPerks = perks.Where(x => x.ClassId == 5 && (x.Checkboxnumber == 6 || x.Checkboxnumber == 7));

            Connection.BeginTransaction();
            try
            {
                foreach (var p in scoundrelPerks)
                {
                    p.Perktext = "Add one [+2] FROST [FROST] card";
                    ClassPerkRepository.InsertOrReplace(p);
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void UpdateEventHistoryPositions()
        {
            Connection.BeginTransaction();
            try
            {
                foreach (DL_Campaign c in CampaignRepository.Get())
                {
                    if (c != null)
                    {
                        List<DL_CampaignEventHistoryLogItem> roadevents = CampaignEventHistoryLogItemRepository.GetEvents(c.Id, 1);
                        List<DL_CampaignEventHistoryLogItem> cityevents = CampaignEventHistoryLogItemRepository.GetEvents(c.Id, 2);
                        foreach (DL_CampaignEventHistoryLogItem evl in roadevents)
                        {
                            evl.Position = roadevents.IndexOf(evl);
                        }
                        foreach (DL_CampaignEventHistoryLogItem evl in cityevents)
                        {
                            evl.Position = cityevents.IndexOf(evl);
                        }
                    }

                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
            }
        }

        private static void MigrateSawbonePerks(List<DL_ClassPerk> perks)
        {
            Connection.BeginTransaction();
            try
            {
                var sawperks = perks.Where(x => x.ClassId == 14);
                var characters = CharacterRepository.Get(false).Where(x => x.ClassId == 15);
                foreach (DL_Character c in characters)
                {
                    var chara = CharacterRepository.Get(c.Id);
                    if (chara.Perks.Any())
                    {
                        foreach (DL_Perk oldperk in chara.Perks.Where(x => x.Checkboxnumber > 0 && x.Checkboxnumber < 16))
                        {
                            var newPerk = sawperks.FirstOrDefault(x => x.ClassId == chara.ClassId - 1 && x.Checkboxnumber == oldperk.Checkboxnumber);

                            if (newPerk != null)
                            {
                                chara.CharacterPerks.Add(newPerk);
                                CharacterRepository.InsertOrReplace(chara);
                            }
                        }
                    }
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }


        private static void UpdateGlobalAchievementNameRiftClosed()
        {
            var ga = AchievementTypeRepository.Get(false).FirstOrDefault(x => x.Name == "The Rift Closed");
            if (ga == null) return;

            Connection.BeginTransaction();
            try
            {
                ga.Name = "The Rift Neutralized";

                AchievementTypeRepository.InsertOrReplace(ga);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void AddSawbonesPerks(List<DL_ClassPerk> perks)
        {
            if (!perks.Any(x => x.ClassId == 14))
            {
                Connection.BeginTransaction();
                try
                {
                    // RM = Rolling Modifier
                    // PU = Push
                    // PI = Pierce
                    // ST = Stun
                    // D = Disarm
                    // I = Imobilize
                    // M = Muddle
                    // W = Wound
                    // T = Add Target
                    // SH = Shield
                    // H = Heal

                    // Sawbones
                    GloomhavenDbHelper.InsertClassPerk(14, "Remove two [-1] cards", 1);
                    GloomhavenDbHelper.InsertClassPerk(14, "Remove two [-1] cards", 2);
                    GloomhavenDbHelper.InsertClassPerk(14, "Remove four [+0] cards", 3);
                    GloomhavenDbHelper.InsertClassPerk(14, "Replace one [+0] card with one [+2] card", 4);
                    GloomhavenDbHelper.InsertClassPerk(14, "Replace one [+0] card with one [+2] card", 5);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add one [RM] [+2] card", 6);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add one [RM] [+2] card", 7);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add one [+1] IMMOBILIZE [I] card", 8);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add one [+1] IMMOBILIZE [I] card", 9);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add two [RM] WOUND [W] cards", 10);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add two [RM] WOUND [W] cards", 11);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add one [RM] STUN [ST] card", 12);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add one [RM] HEAL [H] 1 SELF card", 13);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add one [RM] HEAL [H] 1 SELF card", 14);
                    GloomhavenDbHelper.InsertClassPerk(14, "Add one [+0] Refresh an item card", 15);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }
    }
}
