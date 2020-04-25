using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using SQLite;
using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;
using GloomhavenCampaignTracker.Droid.Data.ConversionClasses;
using Android.Content.Res;

namespace Data
{
    internal class DatabaseUpdateHelper
    {
        private enum VersionTime { Earlier = -1 }
        public static Version Dbversion { get; } = new Version(1, 4, 22);
        public static SQLiteConnection Connection => GloomhavenDbHelper.Connection;
        public static event EventHandler<UpdateSplashScreenLoadingInfoEVentArgs> UpdateLoadingStep;

        protected static void OnUpdateLoadingStep(UpdateSplashScreenLoadingInfoEVentArgs e)
        {
            UpdateLoadingStep?.Invoke(null, e);
        }

        internal static void CheckForUpdates(DL_GlommhavenSettings currentDbVersion)
        {
            var old = Version.Parse(currentDbVersion.Value);          
            if ((VersionTime)old.CompareTo(Dbversion) == VersionTime.Earlier)
            {
                OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Check for databaseupdates"));

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
                    FixNameOfScenario(31, "Plane of Night");
                }

                if ((VersionTime)old.CompareTo(new Version(1, 3, 18)) == VersionTime.Earlier)
                {
                    FixNameOfScenario(54, "Palace of Ice");
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
                    FixNameOfScenario(83, "Shadows Within");
                    AddItem151();
                }
                
                if ((VersionTime)old.CompareTo(new Version(1, 4, 7)) == VersionTime.Earlier)
                {
                    FixSawHealPerk();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 8)) == VersionTime.Earlier)
                {
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

                if ((VersionTime)old.CompareTo(new Version(1, 4, 12)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.12"));
                    FixNameOfScenario(87, "Corrupted Cove");
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 13)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.13"));
                    AddPartyAchievementSunblessed();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 14)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.14"));
                    AddClasses();
                    AddClassAblities();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 15)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.15"));
                    MigrateCharactersToClasses();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 16)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.16"));
                    FixingTyposInPersonalQuestCounters();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 17)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.17"));
                    AddScenarioTreasures();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 18)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.18"));
                    MigrateScenarioTreasures();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 19)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.19"));
                    FixTypoDivinerCard578();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 20)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.20"));
                    AddRegenerateEnhancement();
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 21)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.21"));
                    FixNameOfScenario(59, "Forgotten Grove");
                }

                if ((VersionTime)old.CompareTo(new Version(1, 4, 22)) == VersionTime.Earlier)
                {
                    OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Database update 1.4.22"));
                    FixTreasureScenario54TreasureIsMissing();
                }

                currentDbVersion.Value = Dbversion.ToString();
                GloomhavenSettingsRepository.InsertOrReplace(currentDbVersion);

                OnUpdateLoadingStep(new UpdateSplashScreenLoadingInfoEVentArgs("Finished databaseupdates"));
            }
        }

        private static void FixTreasureScenario54TreasureIsMissing()
        {            
            var treasure25 = ScenarioTreasuresRepository.Get().FirstOrDefault(x => x.TreasureNumber == 25);
            if(treasure25 == null)
            {
                var scenario54 = ScenarioRepository.Get().FirstOrDefault(x => x.Scenarionumber == 54);
                if (scenario54 != null)
                {
                    treasure25 = new DL_ScenarioTreasure
                    {
                        Scenario = scenario54,
                        Scenario_ID = scenario54.Id,
                        TreasureNumber = 25
                    };

                    ScenarioTreasuresRepository.InsertOrReplace(treasure25);                   
                }                
            }
        }

        internal static void CheckIfPASunnblessedExists()
        {
            AddPartyAchievementSunblessed();
        }

        private static void AddRegenerateEnhancement()
        {
            var enhancement = EnhancementRepository.Get().FirstOrDefault(x=>x.EnhancementCode == "[RE2]");
            if(enhancement == null)
            {
                Connection.BeginTransaction();
                try
                {
                    GloomhavenDbHelper.InsertEnhancement("[RE2]", 50);    
                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void FixTypoDivinerCard578()
        {
            var clas = ClassRepository.Get().FirstOrDefault(x => x.ClassId == 18);
            if (clas == null)
            {
                AddDivinerClass();
                Connection.BeginTransaction();
                try
                {
                    var ability578 = clas.Abilities.FirstOrDefault(x => x.ReferenceNumber == 578);

                    if (ability578 != null)
                    {
                        ability578.AbilityName = "Otherworldly Journey";
                        ClassAbilitiesRepository.InsertOrReplace(ability578);

                        Connection.Commit();
                    }
                }
                catch
                {
                    Connection.Rollback();
                }
            }  
        }

        private static void FixingTyposInPersonalQuestCounters()
        {
            Connection.BeginTransaction();
            try
            {
                var counters = PersonalQuestCountersRepository.Get();

                foreach(var counter in counters)
                {
                    if (counter.PersonalQuest.QuestNumber == 513 && counter.CounterName == "Killed Forrest Imps")
                    {
                        counter.CounterName = "Killed Forest Imps";
                        PersonalQuestCountersRepository.InsertOrReplace(counter, false);
                    }
                    if (counter.PersonalQuest.QuestNumber == 521 && counter.CounterName == "Scenarios in Dagger Forrest")
                    {
                        counter.CounterName = "Scenarios in Dagger Forest";
                        PersonalQuestCountersRepository.InsertOrReplace(counter, false);
                    }
                    if (counter.PersonalQuest.QuestNumber == 531 && counter.CounterName == "Dagger Forrest")
                    {
                        counter.CounterName = "Dagger Forest";
                        PersonalQuestCountersRepository.InsertOrReplace(counter, false);
                    }
                    if (counter.PersonalQuest.QuestNumber == 532 && counter.CounterName == "Experienced Retiremets")
                    {
                        counter.CounterName = "Experienced Retirements";
                        PersonalQuestCountersRepository.InsertOrReplace(counter, false);
                    }
                    if (counter.PersonalQuest.QuestNumber == 533 && counter.CounterName == "Lurker")
                    {
                        counter.CounterName = "Lurkers";
                        PersonalQuestCountersRepository.InsertOrReplace(counter, false);
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

        private static void MigrateCharactersToClasses()
        {
            Connection.BeginTransaction();
            try
            {
                var classes = ClassRepository.Get();
                var characters = CharacterRepository.Get();

                foreach (var character in characters)
                {
                    var charClass = classes.FirstOrDefault(x => x.ClassId == character.ClassId);

                    if (charClass == null) continue;

                    character.DL_Class = charClass;
                    character.ID_Class = charClass.Id;

                    if (character.CharacterAbilities == null) character.CharacterAbilities = new List<DL_CharacterAbility>();

                    foreach (var oldCharacterAbility in character.Abilities)
                    {
                        if (oldCharacterAbility.ReferenceNumber != 0)
                        {
                            var classAbility = charClass.Abilities.FirstOrDefault(x => 
                                x.ReferenceNumber == oldCharacterAbility.ReferenceNumber && 
                                x.DL_Class.ClassId == character.ClassId);

                            if (classAbility == null) continue;

                            var newCharacterAbility = new DL_CharacterAbility()
                            {
                                Ability = classAbility,
                                Character = character,
                                ID_Character = character.Id,
                                ID_ClassAbility = classAbility.Id,
                                AbilityEnhancements = new List<DL_CharacterAbilityEnhancement>()
                            };

                            foreach (var enhancement in oldCharacterAbility.Enhancements)
                            {
                                var abilityenhancement = new DL_CharacterAbilityEnhancement()
                                {
                                    CharacterAbility = newCharacterAbility,
                                    Enhancement = enhancement.Enhancement,
                                    ID_CharacterAbility = newCharacterAbility.Id,
                                    ID_Enhancement = enhancement.ID_Enhancement,
                                    IsTop = enhancement.IsTop,
                                    SlotNumber = enhancement.SlotNumber
                                };

                                newCharacterAbility.AbilityEnhancements.Add(abilityenhancement);
                            }

                            character.CharacterAbilities.Add(newCharacterAbility);
                        }
                    }

                    foreach (var firstlevelability in charClass.Abilities.Where(x => x.Level == 1))
                    {
                        if (character.CharacterAbilities.Any(x => x.ID_ClassAbility == firstlevelability.Id)) continue;

                        var characterAbility = new DL_CharacterAbility()
                        {
                            Ability = firstlevelability,
                            Character = character,
                            ID_Character = character.Id,
                            ID_ClassAbility = firstlevelability.Id
                        };

                        character.CharacterAbilities.Add(characterAbility);
                    }

                    CharacterRepository.InsertOrReplace(character);
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void MigrateScenarioTreasures()
        {
            Connection.BeginTransaction();
            try
            {
                var campScenarios = CampaignUnlockedScenarioRepository.Get();

                foreach(var campscen in campScenarios)
                {
                    if(campscen.Scenario.Treasures.Any())
                    {
                        foreach (var scenTreas in campscen.Scenario.Treasures)
                        {
                            if (campscen.CampaignScenarioTreasures == null)
                                campscen.CampaignScenarioTreasures = new List<DL_CampaignScenarioTreasure>();

                            if (!campscen.CampaignScenarioTreasures.Any(x => x.ScenarioTreasure.TreasureNumber == scenTreas.TreasureNumber))
                            {
                                var cst = new DL_CampaignScenarioTreasure()
                                {
                                    ScenarioTreasure = scenTreas,
                                    ScenarioTreasure_ID = scenTreas.Id,
                                    UnlockedScenario = campscen,
                                    CampaignScenario_ID = campscen.Id
                                };

                                var y = campscen.ScenarioTreasures.FirstOrDefault(x => x.Number == scenTreas.TreasureNumber);

                                if (y != null)
                                    cst.Looted = y.Looted;

                                campscen.CampaignScenarioTreasures.Add(cst);
                                CampaignUnlockedScenarioRepository.InsertOrReplace(campscen);
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

        internal static void AddScenarioTreasures()
        {
            Connection.BeginTransaction();
            try
            {
                // read from JSON
                AssetManager assets = Android.App.Application.Context.Assets;
                var asset = Android.App.Application.Context.Assets.Open("raw_data/ScenarioTreaures.json");
                var scenarioTreasures = JSONConverter.LoadJson<ScenarioTreaures>(asset);

                var scenarios = ScenarioRepository.Get();
                var scenTreasures = ScenarioTreasuresRepository.Get();

                foreach (var scenariotreasure in scenarioTreasures.scenariotreasures)
                {
                    var currentScenario = scenarios.FirstOrDefault(x => x.Scenarionumber == scenariotreasure.scenarionumber);

                    if (currentScenario == null) continue;

                    if (scenTreasures.Any(x => x.Scenario.Scenarionumber == scenariotreasure.scenarionumber && 
                                            x.TreasureNumber == scenariotreasure.treasurenumber)) continue;

                    var treasure = new DL_ScenarioTreasure()
                    {
                        Scenario = currentScenario,
                        Scenario_ID = currentScenario.Id,
                        TreasureContent = "",
                        TreasureNumber = scenariotreasure.treasurenumber
                    };

                    ScenarioTreasuresRepository.InsertOrReplace(treasure);                    
                }

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }


        internal static void AddClassAblities()
        {
            Connection.BeginTransaction();
            try
            {
                // read from JSON
                AssetManager assets = Android.App.Application.Context.Assets;
                var asset = Android.App.Application.Context.Assets.Open("raw_data/Classabilities.json");
                var classabilities = JSONConverter.LoadJson<ClassAbilites>(asset);

                var classes = ClassRepository.Get();
                var classAbs = ClassAbilitiesRepository.Get();
                foreach (var classAb in classabilities.classabilities)
                {
                    var currentClass = classes.FirstOrDefault(x => x.ClassId == classAb.classid+1);

                    if (currentClass == null) continue ;

                    if (classAbs.Any(x => x.ID_Class == currentClass.Id)) continue;

                    foreach(var ability in classAb.Abilities)
                    {
                        var classAbility = new DL_ClassAbility()
                        {
                            AbilityName = ability.abilityName,
                            DL_Class = currentClass,
                            ID_Class = currentClass.Id,
                            Level = ability.level,
                            ReferenceNumber = ability.referenceNumber
                        };

                        ClassAbilitiesRepository.InsertOrReplace(classAbility);
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

        internal static void AddClasses()
        {
            var classes = ClassRepository.Get();

            if (classes.Any())
            {
                Connection.DropTable<DL_Class>();
                Connection.CreateTable<DL_Class>();
            }

            classes = ClassRepository.Get();

            Connection.BeginTransaction();
            try
            {
                if (!classes.Any(x=>x.ClassId == 1))
                {
                    // add Brute
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 1,
                        ClassName = "Inox Brute",
                        ClassShorty = "BR",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 2))
                {
                    // add Tinkerer
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 2,
                        ClassName = "Quatryl Tinkerer",
                        ClassShorty = "TI",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 3))
                {
                    // add Orchid Spellweaver
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 3,
                        ClassName = "Orchid Spellweaver",
                        ClassShorty = "SW",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 4))
                {
                    // add Human Scoundrel
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 4,
                        ClassName = "Human Scoundrel",
                        ClassShorty = "SC",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 5))
                {
                    // add Savvas Cragheart
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 5,
                        ClassName = "Savvas Cragheart",
                        ClassShorty = "CH",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 6))
                {
                    // add Vermling Mindthief
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 6,
                        ClassName = "Vermling Mindthief",
                        ClassShorty = "MT",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 7))
                {
                    // add Valrath Sunkeeper
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 7,
                        ClassName = "Valrath Sunkeeper",
                        ClassShorty = "SK",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 8))
                {
                    // add Valrath Quatermaster
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 8,
                        ClassName = "Valrath Quatermaster",
                        ClassShorty = "QM",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 9))
                {
                    // add Aesther Summoner
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 9,
                        ClassName = "Aesther Summoner",
                        ClassShorty = "SU",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 10))
                {
                    // add Aesther Nightshround
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 10,
                        ClassName = "Aesther Nightshround",
                        ClassShorty = "NS",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 11))
                {
                    // add Harrower Plagueherald
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 11,
                        ClassName = "Harrower Plagueherald",
                        ClassShorty = "PH",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }


                if (!classes.Any(x => x.ClassId == 12))
                {
                    // add Inox Berserker
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 12,
                        ClassName = "Inox Berserker",
                        ClassShorty = "BE",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 13))
                {
                    // add Quatryl Soothsinger
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 13,
                        ClassName = "Quatryl Soothsinger",
                        ClassShorty = "SS",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 14))
                {
                    // add Orchid Doomstalker
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 14,
                        ClassName = "Orchid Doomstalker",
                        ClassShorty = "DS",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 15))
                {
                    // add Human Sawbones
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 15,
                        ClassName = "Human Sawbones",
                        ClassShorty = "SB",
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
                        ClassName = "Savvas Elementalist",
                        ClassShorty = "EL",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 17))
                {
                    // add Vermling Beasttyrant
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 17,
                        ClassName = "Vermling Beasttyrant",
                        ClassShorty = "BT",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 18))
                {
                    // add Harrower Bladeswarm
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 18,
                        ClassName = "Harrower Bladeswarm",
                        ClassShorty = "BS",
                        Perks = new List<DL_ClassPerks>()
                    };

                    SaveClass(cl);
                }

                if (!classes.Any(x => x.ClassId == 19))
                {
                    // add Aesther Diviner
                    var cl = new DL_Class
                    {
                        Abilities = new List<DL_ClassAbility>(),
                        Characters = new List<DL_Character>(),
                        ClassId = 19,
                        ClassName = "Aesther Diviner",
                        ClassShorty = "DR",
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

        private static void FixNameOfScenario(int scenarioNumber,string scenarioName)
        {
            var scenario = ScenarioRepository.Get(true).FirstOrDefault(x => x.Scenarionumber == scenarioNumber);
            if (scenario == null) return;

            Connection.BeginTransaction();
            try
            {
                scenario.Name = scenarioName;

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
                    catch
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

        //private static void MigrateAbillities()
        //{
        //    Connection.BeginTransaction();
        //    try
        //    {
        //        foreach (DL_Ability a in AbilityRepository.Get())
        //        {
        //            if (int.TryParse(a.Name, out int aNumber))
        //            {
        //                a.ReferenceNumber = aNumber;
        //            }

        //            AbilityRepository.InsertOrReplace(a);
        //        }

        //        Connection.Commit();
        //    }
        //    catch
        //    {
        //        Connection.Rollback();
        //        throw;
        //    }
        //}

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
            var partyachievement = PartyAchievementRepository.Get().FirstOrDefault(x => x.InternalNumber == 28 && x.Name == "Sun Blessed");

            if (partyachievement == null)
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
            else
            {
                if(partyachievement.ContentOfPack == 2)
                {
                    partyachievement.ContentOfPack = 1;
                    PartyAchievementRepository.InsertOrReplace(partyachievement);
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
