using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using SQLite;
using Java.IO;
using System.Diagnostics;
using Data;
using GloomhavenCampaignTracker.Shared.Data.Entities.Classdesign;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public class GloomhavenDbHelper
    {
        // Database Info
        internal const string DatabaseName = "GloomhavenCampaignTracker.db3";
        private static SQLiteConnection _conn;
        private enum VersionTime { Earlier = -1 }

        internal static string DatabaseFilePath
        {
            get
            {
                // Just use whatever directory SpecialFolder.Personal returns
                var libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var path = Path.Combine(libraryPath, DatabaseName);
                return path;
            }
        }

        public static SQLiteConnection Connection
        {
            get
            {
#if __ANDROID__
                return _conn ?? (_conn = new SQLiteConnection(DatabaseFilePath));
#endif

#if __IOS__
                
#endif
            }
        }

        internal static void ResetConnection()
        {
            _conn = null;
        }

        public static void InitDb()
        {
            CreateTables();

            var currentDbVersion = GloomhavenSettingsRepository.Get().FirstOrDefault(x => x.Name == "DBVERSION");

            if (currentDbVersion == null)
            {
                currentDbVersion = CreateNewDB();
            }
            else
            {
                DatabaseUpdateHelper.CheckIfAllPerksExists();
                DatabaseUpdateHelper.CheckForUpdates(currentDbVersion);
            }
        }


        private static void CreateTables()
        {
            CreateTableGlommhavenSettings();
            CreateTableAchievement();
            CreateTableAchievementType();
            CreateTableCampaignGlobalAchievement();
            CreateTableCampaign();
            CreateTableScenario();
            CreateTableCampaignUnlockedScenario();
            CreateTableCampaignParty();
            CreateTableAbility();
            CreateTableCharacter();
            CreateTablePartyAchievement();
            CreateTableCampaignPartyAchievement();
            CreateTableCampaignUnlocks();
            CreateTablePerk();
            CreateTableCampaignEventHistoryLogItem();
            CreateTableTreasure();
            CreateTableItem();
            CreateTablePersonalQuest();
            CreateTableCharacterItem();
            CreateTableCampaignUnlockedItem();
            CreateTableClassPerk();
            CreateTableCharacterPerk();
            CreateTableEnhancement();
            CreateTableAbilityEnhancement();
            CreateTablePersonalQuestCounter();
            CreateTableCharacterPersonalQuestCounter();

            Connection.CreateTable<DL_Class>();
            Connection.CreateTable<DL_ClassAbility>();
            Connection.CreateTable<DL_ClassPerks>();
            Connection.CreateTable<DL_CharacterAbility>();
            Connection.CreateTable<DL_CharacterAbilityEnhancement>();
            Connection.CreateTable<DL_CharacterPerks>();
        }

        private static DL_GlommhavenSettings CreateNewDB()
        {
            DL_GlommhavenSettings currentDbVersion;
            CreateTables();
            currentDbVersion = new DL_GlommhavenSettings
            {
                Name = "DBVERSION",
                Value = DatabaseUpdateHelper.Dbversion.ToString()
            };
            GloomhavenSettingsRepository.InsertOrReplace(currentDbVersion);

            FillDb();
            return currentDbVersion;
        }

        internal static void AddRegionsToScenarios()
        {
            var scenarios = ScenarioRepository.Get(false);
            if (scenarios == null || scenarios.Count == 0) return;

            Connection.BeginTransaction();
            try
            {
                var sdc = new ScenarioDataService();

                // 1 = Gloomhaven
                // 2 = Dagger Forest
                // 3 = Copperneck Mountains
                // 4 = Lintgering Swamps
                // 5 = Corpsewood
                // 6 = Watcher Mountains
                // 7 = Misty Sea

                sdc.UpdateScenarioRegion(1, 5);
                sdc.UpdateScenarioRegion(2, 5);
                sdc.UpdateScenarioRegion(3, 2);
                sdc.UpdateScenarioRegion(4, 0);
                sdc.UpdateScenarioRegion(5, 0);
                sdc.UpdateScenarioRegion(6, 0);
                sdc.UpdateScenarioRegion(7, 3);
                sdc.UpdateScenarioRegion(8, 1);
                sdc.UpdateScenarioRegion(9, 6);
                sdc.UpdateScenarioRegion(10, 0);
                sdc.UpdateScenarioRegion(11, 1);
                sdc.UpdateScenarioRegion(12, 1);
                sdc.UpdateScenarioRegion(13, 6);
                sdc.UpdateScenarioRegion(14, 3);
                sdc.UpdateScenarioRegion(15, 3);
                sdc.UpdateScenarioRegion(16, 3);
                sdc.UpdateScenarioRegion(17, 7);
                sdc.UpdateScenarioRegion(18, 1);
                sdc.UpdateScenarioRegion(19, 4);
                sdc.UpdateScenarioRegion(20, 0);
                sdc.UpdateScenarioRegion(21, 0);
                sdc.UpdateScenarioRegion(22, 0);
                sdc.UpdateScenarioRegion(23, 1);
                sdc.UpdateScenarioRegion(24, 3);
                sdc.UpdateScenarioRegion(25, 3);
                sdc.UpdateScenarioRegion(26, 1);
                sdc.UpdateScenarioRegion(27, 0);
                sdc.UpdateScenarioRegion(28, 2);
                sdc.UpdateScenarioRegion(29, 2);
                sdc.UpdateScenarioRegion(30, 7);
                sdc.UpdateScenarioRegion(31, 1);
                sdc.UpdateScenarioRegion(32, 4);
                sdc.UpdateScenarioRegion(33, 3);
                sdc.UpdateScenarioRegion(34, 3);
                sdc.UpdateScenarioRegion(35, 1);
                sdc.UpdateScenarioRegion(36, 1);
                sdc.UpdateScenarioRegion(37, 7);
                sdc.UpdateScenarioRegion(38, 2);
                sdc.UpdateScenarioRegion(39, 3);
                sdc.UpdateScenarioRegion(40, 3);
                sdc.UpdateScenarioRegion(41, 3);
                sdc.UpdateScenarioRegion(42, 3);
                sdc.UpdateScenarioRegion(43, 2);
                sdc.UpdateScenarioRegion(44, 2);
                sdc.UpdateScenarioRegion(45, 4);
                sdc.UpdateScenarioRegion(46, 3);
                sdc.UpdateScenarioRegion(47, 7);
                sdc.UpdateScenarioRegion(48, 2);
                sdc.UpdateScenarioRegion(49, 4);
                sdc.UpdateScenarioRegion(50, 1);
                sdc.UpdateScenarioRegion(51, 1);
                sdc.UpdateScenarioRegion(52, 1);
                sdc.UpdateScenarioRegion(53, 0);
                sdc.UpdateScenarioRegion(54, 3);
                sdc.UpdateScenarioRegion(55, 2);
                sdc.UpdateScenarioRegion(56, 2);
                sdc.UpdateScenarioRegion(57, 1);
                sdc.UpdateScenarioRegion(58, 1);
                sdc.UpdateScenarioRegion(59, 2);
                sdc.UpdateScenarioRegion(60, 1);
                sdc.UpdateScenarioRegion(61, 4);
                sdc.UpdateScenarioRegion(62, 0);
                sdc.UpdateScenarioRegion(63, 6);
                sdc.UpdateScenarioRegion(64, 7);
                sdc.UpdateScenarioRegion(65, 6);
                sdc.UpdateScenarioRegion(66, 3);
                sdc.UpdateScenarioRegion(67, 0);
                sdc.UpdateScenarioRegion(68, 4);
                sdc.UpdateScenarioRegion(69, 0);
                sdc.UpdateScenarioRegion(70, 7);
                sdc.UpdateScenarioRegion(71, 0);
                sdc.UpdateScenarioRegion(72, 5);
                sdc.UpdateScenarioRegion(73, 6);
                sdc.UpdateScenarioRegion(74, 7);
                sdc.UpdateScenarioRegion(75, 5);
                sdc.UpdateScenarioRegion(76, 6);
                sdc.UpdateScenarioRegion(77, 1);
                sdc.UpdateScenarioRegion(78, 1);
                sdc.UpdateScenarioRegion(79, 7);
                sdc.UpdateScenarioRegion(80, 6);
                sdc.UpdateScenarioRegion(81, 2);
                sdc.UpdateScenarioRegion(82, 6);
                sdc.UpdateScenarioRegion(83, 1);
                sdc.UpdateScenarioRegion(84, 3);
                sdc.UpdateScenarioRegion(85, 6);
                sdc.UpdateScenarioRegion(86, 1);
                sdc.UpdateScenarioRegion(87, 0);
                sdc.UpdateScenarioRegion(88, 1);
                sdc.UpdateScenarioRegion(89, 1);
                sdc.UpdateScenarioRegion(90, 0);
                sdc.UpdateScenarioRegion(91, 2);
                sdc.UpdateScenarioRegion(92, 1);
                sdc.UpdateScenarioRegion(93, 7);
                sdc.UpdateScenarioRegion(94, 5);
                sdc.UpdateScenarioRegion(95, 0);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void CreateTableCharacterPersonalQuestCounter()
        {
            Connection.CreateTable<DL_CharacterPersonalQuestCounter>();
        }

        private static void CreateTablePersonalQuestCounter()
        {
            Connection.CreateTable<DL_PersonalQuestCounter>();
        }

        private static void CreateTableAbilityEnhancement()
        {
            Connection.CreateTable<DL_AbilityEnhancement>();
        }

        private static void CreateTableEnhancement()
        {
            Connection.CreateTable<DL_Enhancement>();
        }

        private static void CreateTableCharacterPerk()
        {
            Connection.CreateTable<DL_CharacterPerk>();
        }

        private static void CreateTableClassPerk()
        {
            Connection.CreateTable<DL_ClassPerk>();
        }

        private static void CreateTableCampaignUnlockedItem()
        {
            Connection.CreateTable<DL_CampaignUnlockedItem>();
        }

        private static void CreateTableCharacterItem()
        {
            Connection.CreateTable<DL_CharacterItem>();
        }

        private static void CreateTablePersonalQuest()
        {
            Connection.CreateTable<DL_PersonalQuest>();
        }

        private static void CreateTableItem()
        {
            Connection.CreateTable<DL_Item>();
        }

        private static void CreateTableTreasure()
        {
            Connection.CreateTable<DL_Treasure>();
        }

        private static void CreateTableCampaignEventHistoryLogItem()
        {
            Connection.CreateTable<DL_CampaignEventHistoryLogItem>();
        }

        private static void CreateTablePerk()
        {
            Connection.CreateTable<DL_Perk>();
        }

        private static void CreateTableCampaignUnlocks()
        {
            Connection.CreateTable<DL_CampaignUnlocks>();
        }

        private static void CreateTableCampaignPartyAchievement()
        {
            Connection.CreateTable<DL_CampaignPartyAchievement>();
        }

        private static void CreateTablePartyAchievement()
        {
            Connection.CreateTable<DL_PartyAchievement>();
        }

        private static void CreateTableCharacter()
        {
            Connection.CreateTable<DL_Character>();
        }

        private static void CreateTableAbility()
        {
            Connection.CreateTable<DL_Ability>();
        }

        private static void CreateTableCampaignParty()
        {
            Connection.CreateTable<DL_CampaignParty>();
        }

        private static void CreateTableCampaignUnlockedScenario()
        {
            Connection.CreateTable<DL_CampaignUnlockedScenario>();
        }

        private static void CreateTableScenario()
        {
            Connection.CreateTable<DL_Scenario>();
        }

        private static void CreateTableCampaign()
        {
            Connection.CreateTable<DL_Campaign>();
        }

        private static void CreateTableCampaignGlobalAchievement()
        {
            Connection.CreateTable<DL_CampaignGlobalAchievement>();
        }

        private static void CreateTableAchievementType()
        {
            Connection.CreateTable<DL_AchievementType>();
        }

        private static void CreateTableAchievement()
        {
            Connection.CreateTable<DL_Achievement>();
        }

        private static void CreateTableGlommhavenSettings()
        {
            Connection.CreateTable<DL_GlommhavenSettings>();
        }

        public static void FillDb()
        {
            FillAchievements();
            FillScenarios();
            FillPartyAchievements();
            FillPersonalQuests();
            FillItems();
            FillClassPerks();
            FillEnhancement();
            FillPersonalQuestCounters();

            AddRegionsToScenarios();
        }

        internal static void FillPersonalQuestCounters()
        {
            if (!PersonalQuestCountersRepository.Get(false).Any())
            {
                Connection.BeginTransaction();
                try
                {
                    var pqs = PersonalQuestRepository.Get(false);

                    foreach (var pq in pqs)
                    {
                        switch (pq.QuestNumber)
                        {
                            case 510:
                                InsertPQCounter(pq, "Crypt scenarios", 3, 52);
                                InsertPQCounter(pq, "Completed quest starting with scenario #52", 1, 0);
                                break;
                            case 511:
                                InsertPQCounter(pq, "Helmets", 2, 0);
                                InsertPQCounter(pq, "Armor", 2, 0);
                                InsertPQCounter(pq, "Boots", 2, 0);
                                InsertPQCounter(pq, "Hands", 3, 0);
                                InsertPQCounter(pq, "Small items", 4, 0);
                                break;
                            case 512:
                                InsertPQCounter(pq, "Gold", 200, 0);
                                break;
                            case 513:
                                InsertPQCounter(pq, "Killed Forrest Imps", 8, 59);
                                InsertPQCounter(pq, "Completed quest starting with scenario #59", 1, 0);
                                break;
                            case 514:
                                InsertPQCounter(pq, "Exhausted partymembers", 15, 0);
                                break;
                            case 515:
                                InsertPQCounter(pq, "Killed Bandits/Cultists", 20, 0);
                                break;
                            case 516:
                                InsertPQCounter(pq, "Killed Vermlings", 15, 0);
                                break;
                            case 517:
                                InsertPQCounter(pq, "Killed different Monsters", 20, 0);
                                break;
                            case 518:
                                InsertPQCounter(pq, "Completed Scenarios", 15, 0);
                                break;
                            case 519:
                                InsertPQCounter(pq, "Gained Checkmarks", 15, 0);
                                break;
                            case 520:
                                InsertPQCounter(pq, "Killed 'Living' Monsters with Skullbane Axe", 7, 0);
                                break;
                            case 521:
                                InsertPQCounter(pq, "Scenarios in Dagger Forrest", 3, 55);
                                InsertPQCounter(pq, "Completed quest starting with scenario #55", 1, 0);
                                break;
                            case 522:
                                InsertPQCounter(pq, "Side Scenarios (> 51)", 6, 0);
                                break;
                            case 523:
                                InsertPQCounter(pq, "Flame Demon", 1, 0);
                                InsertPQCounter(pq, "Frost Demon", 1, 0);
                                InsertPQCounter(pq, "Wind Demon", 1, 0);
                                InsertPQCounter(pq, "Earth Demon", 1, 0);
                                InsertPQCounter(pq, "Night Demon", 1, 0);
                                InsertPQCounter(pq, "Sun Demon", 1, 0);
                                break;
                            case 524:
                                InsertPQCounter(pq, "Killed Elite Monsters", 20, 0);
                                break;
                            case 525:
                                InsertPQCounter(pq, "Donated Gold", 120, 0);
                                break;
                            case 526:
                                InsertPQCounter(pq, "Completed Scenarios in Gloomhaven", 4, 57);
                                InsertPQCounter(pq, "Completed quest starting with scenario #57", 1, 0);
                                break;
                            case 527:
                                InsertPQCounter(pq, "Become Exhausted", 12, 0);
                                break;
                            case 528:
                                InsertPQCounter(pq, "Boss Scenarios", 4, 0);
                                break;
                            case 529:
                                InsertPQCounter(pq, "Completed Scenarios in Lingering Swamp", 2, 61);
                                InsertPQCounter(pq, "Completed quest starting with scenario #61", 1, 0);
                                break;
                            case 530:
                                InsertPQCounter(pq, "Enhancements", 4, 0);
                                break;
                            case 531:
                                InsertPQCounter(pq, "Gloomhaven", 1, 0);
                                InsertPQCounter(pq, "Dagger Forrest", 1, 0);
                                InsertPQCounter(pq, "Lingering Swamp", 1, 0);
                                InsertPQCounter(pq, "Watcher Mountains", 1, 0);
                                InsertPQCounter(pq, "Copperneck Mountains", 1, 0);
                                InsertPQCounter(pq, "Misty Sea", 1, 0);
                                break;
                            case 532:
                                InsertPQCounter(pq, "Experienced Retiremets", 2, 0);
                                break;
                            case 533:
                                InsertPQCounter(pq, "Oozes", 3, 0);
                                InsertPQCounter(pq, "Lurker", 3, 0);
                                InsertPQCounter(pq, "Spitting Drakes", 3, 0);
                                break;
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

        internal static DL_PersonalQuestCounter InsertPQCounter(DL_PersonalQuest pq, string countername, int counter, int scenario)
        {
            var item = new DL_PersonalQuestCounter
            {
                CounterName = countername,
                CounterScenarioUnlock = scenario,
                CounterValue = counter,
                PersonalQuest_ID = pq.Id,
                PersonalQuest = pq
            };

            PersonalQuestCountersRepository.InsertOrReplace(item, false);

            return item;
        }

        internal static void FillEnhancement()
        {
            if (!EnhancementRepository.Get(false).Any())
            {
                Connection.BeginTransaction();
                try
                {
                    // base +1
                    InsertEnhancement("[MOVE][e+1]", 30);
                    InsertEnhancement("[ATTACK][e+1]", 50);
                    InsertEnhancement("[RANGE][e+1]", 30);
                    InsertEnhancement("[SH][e+1]", 100);
                    InsertEnhancement("[PU][e+1]", 30);
                    InsertEnhancement("[PUL][e+1]", 30);
                    InsertEnhancement("[PI][e+1]", 30);
                    InsertEnhancement("[RET][e+1]", 100);
                    InsertEnhancement("[H][e+1]", 30);
                    InsertEnhancement("[T][e+1]", 50);

                    // summon +1
                    InsertEnhancement("Summon [MOVE][e+1]", 100);
                    InsertEnhancement("Summon [ATTACK][e+1]", 100);
                    InsertEnhancement("Summon [RANGE][e+1]", 50);
                    InsertEnhancement("Summon [H][e+1]", 50);

                    // effects
                    InsertEnhancement("[PO]", 75);
                    InsertEnhancement("[W]", 75);
                    InsertEnhancement("[M]", 50);
                    InsertEnhancement("[I]", 100);
                    InsertEnhancement("[D]", 150);
                    InsertEnhancement("[C]", 75);
                    InsertEnhancement("[STR]", 50);
                    InsertEnhancement("[B]", 50);
                    InsertEnhancement("[J]", 50);

                    // elements
                    InsertEnhancement("[EARTH]", 100);
                    InsertEnhancement("[WIND]", 100);
                    InsertEnhancement("[FIRE]", 100);
                    InsertEnhancement("[LIGHT]", 100);
                    InsertEnhancement("[DARK]", 100);
                    InsertEnhancement("[FROST]", 100);
                    InsertEnhancement("[ANY]", 150);

                    InsertEnhancement("[HEX]", 200);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        private static void InsertEnhancement(string enhancementCode, int basecosts)
        {
            var item = new DL_Enhancement
            {
                EnhancementCode = enhancementCode,
                Basecosts = basecosts
            };

            EnhancementRepository.InsertOrReplace(item);
        }

        internal static void FillClassPerks()
        {
            if (!ClassPerkRepository.Get().Any())
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

                    // Brute
                    InsertClassPerk(0, "Remove two [-1] cards", 1);
                    InsertClassPerk(0, "Replace one [-1] card with one [+1] card", 2);
                    InsertClassPerk(0, "Add two [+1] cards", 3);
                    InsertClassPerk(0, "Add two [+1] cards", 4);
                    InsertClassPerk(0, "Add one [+3] card", 5);
                    InsertClassPerk(0, "Add three [RM] PUSH [PU] 1 cards", 6);
                    InsertClassPerk(0, "Add three [RM] PUSH [PU] 1 cards", 7);
                    InsertClassPerk(0, "Add two [RM] PIERCE [PI] 3 cards", 8);
                    InsertClassPerk(0, "Add one [RM] STUN [ST] card", 9);
                    InsertClassPerk(0, "Add one [RM] STUN [ST] card", 10);
                    InsertClassPerk(0, "Add one [RM] DISARM [D] card and one [RM] MUDDLE [M] card", 11);
                    InsertClassPerk(0, "Add one [RM] ADD TARGET [T] card", 12);
                    InsertClassPerk(0, "Add one [RM] ADD TARGET [T] card", 13);
                    InsertClassPerk(0, "Add one [+1] Shield [SH] 1, Self card", 14);
                    InsertClassPerk(0, "Ignore negative item effects and add one [+1] card", 15);

                    // Tinkerer
                    InsertClassPerk(1, "Remove two [-1] cards", 1);
                    InsertClassPerk(1, "Remove two [-1] cards", 2);
                    InsertClassPerk(1, "Replace one [-2] card with one [+0] card", 3);
                    InsertClassPerk(1, "Add two [+1] cards", 4);
                    InsertClassPerk(1, "Add one [+3] card", 5);
                    InsertClassPerk(1, "Add two [RM] [FIRE] cards", 6);
                    InsertClassPerk(1, "Add three [RM] MUDDLE [M] cards", 7);
                    InsertClassPerk(1, "Add one [+1] WOUND [W] card", 8);
                    InsertClassPerk(1, "Add one [+1] WOUND [W] card", 9);
                    InsertClassPerk(1, "Add one [+1] IMMOBILIZE [I] card", 10);
                    InsertClassPerk(1, "Add one [+1] IMMOBILIZE [I] card", 11);
                    InsertClassPerk(1, "Add one [+1] Heal [H]2, self card", 12);
                    InsertClassPerk(1, "Add one [+1] Heal [H]2, self card", 13);
                    InsertClassPerk(1, "Add one [+0] ADD TARGET [T] card", 14);
                    InsertClassPerk(1, "Ignore negative scenario effects", 15);

                    // Orchid Spellweaver
                    InsertClassPerk(2, "Remove four [+0] cards", 1);
                    InsertClassPerk(2, "Replace one [-1] card with one [+1] card", 2);
                    InsertClassPerk(2, "Replace one [-1] card with one [+1] card", 3);
                    InsertClassPerk(2, "Add two [+1] cards", 4);
                    InsertClassPerk(2, "Add two [+1] cards", 5);
                    InsertClassPerk(2, "Add one [+0] STUN [ST] card", 6);
                    InsertClassPerk(2, "Add one [+1] WOUND [W] card", 7);
                    InsertClassPerk(2, "Add one [+1] IMMOBILIZE [I] card", 8);
                    InsertClassPerk(2, "Add one [+1] CURSE [C] card", 9);
                    InsertClassPerk(2, "Add one [+2] [FIRE] card", 10);
                    InsertClassPerk(2, "Add one [+2] [FIRE] card", 11);
                    InsertClassPerk(2, "Add one [+2] [FROST] card", 12);
                    InsertClassPerk(2, "Add one [+2] [FROST] card", 13);
                    InsertClassPerk(2, "Add one [RM] [EARTH] and one [RM] [WIND] card", 14);
                    InsertClassPerk(2, "Add one [RM] [LIGHT] and one [RM] [DARK] card", 15);

                    // Human Scoundrel
                    InsertClassPerk(3, "Remove two [-1] cards", 1);
                    InsertClassPerk(3, "Remove two [-1] cards", 2);
                    InsertClassPerk(3, "Remove four [+0] cards", 3);
                    InsertClassPerk(3, "Replace one [-2] card with one [+0] card", 4);
                    InsertClassPerk(3, "Replace one [-1] card with one [+1] card", 5);
                    InsertClassPerk(3, "Replace one [+0] card with one [+2] card", 6);
                    InsertClassPerk(3, "Replace one [+0] card with one [+2] card", 7);
                    InsertClassPerk(3, "Add two [RM] [+1] cards", 8);
                    InsertClassPerk(3, "Add two [RM] [+1] cards", 9);
                    InsertClassPerk(3, "Add two [RM] PIERCE [PI] 3 cards", 10);
                    InsertClassPerk(3, "Add two [RM] POISON [PO] cards", 11);
                    InsertClassPerk(3, "Add two [RM] POISON [PO] cards", 12);
                    InsertClassPerk(3, "Add two [RM] MUDDLE [M] cards", 13);
                    InsertClassPerk(3, "Add one [RM] INVISIBLE [IN] card", 14);
                    InsertClassPerk(3, "Ignore negative scenario effects", 15);

                    // Savvas Cragheart
                    InsertClassPerk(4, "Remove four [+0] cards", 1);
                    InsertClassPerk(4, "Replace one [-1] card with one [+1] card", 2);
                    InsertClassPerk(4, "Replace one [-1] card with one [+1] card", 3);
                    InsertClassPerk(4, "Replace one [-1] card with one [+1] card", 4);
                    InsertClassPerk(4, "Add one [-2] card and two [+2] cards", 5);
                    InsertClassPerk(4, "Add one [+1] IMMOBILIZE [I] card", 6);
                    InsertClassPerk(4, "Add one [+1] IMMOBILIZE [I] card", 7);
                    InsertClassPerk(4, "Add one [+2] MUDDLE [M] card", 8);
                    InsertClassPerk(4, "Add one [+2] MUDDLE [M] card", 9);
                    InsertClassPerk(4, "Add two [RM] PUSH [PU] 2 cards", 10);
                    InsertClassPerk(4, "Add two [RM] [EARTH] cards", 11);
                    InsertClassPerk(4, "Add two [RM] [EARTH] cards", 12);
                    InsertClassPerk(4, "Add two [RM] [WIND] cards", 13);
                    InsertClassPerk(4, "Ignore negative item effects", 14);
                    InsertClassPerk(4, "Ignore negative scenario effects", 15);

                    // Vermling Mindthief
                    InsertClassPerk(5, "Remove two [-1] cards", 1);
                    InsertClassPerk(5, "Remove two [-1] cards", 2);
                    InsertClassPerk(5, "Remove four [+0] cards", 3);
                    InsertClassPerk(5, "Replace two [+1] cards with two [+2] cards", 4);
                    InsertClassPerk(5, "Replace one [-2] card with one [+0] card", 5);
                    InsertClassPerk(5, "Add one [+2] FROST card", 6);
                    InsertClassPerk(5, "Add one [+2] FROST card", 7);
                    InsertClassPerk(5, "Add two [RM] [+1] cards", 8);
                    InsertClassPerk(5, "Add two [RM] [+1] cards", 9);
                    InsertClassPerk(5, "Add three [RM] PULL [PUL] 1 cards", 10);
                    InsertClassPerk(5, "Add three [RM] MUDDLE [M] cards", 11);
                    InsertClassPerk(5, "Add two [RM] IMMOBILIZE [I] cards", 12);
                    InsertClassPerk(5, "Add one [RM] STUN [ST] card", 13);
                    InsertClassPerk(5, "Add one [RM] DISARM [D] card and one [RM] MUDDLE [M] card ", 14);
                    InsertClassPerk(5, "Ignore negative scenario effects", 15);

                    // Valrath Sunkeeper
                    InsertClassPerk(6, "Remove two [-1] cards", 1);
                    InsertClassPerk(6, "Remove two [-1] cards", 2);
                    InsertClassPerk(6, "Remove four [+0] cards", 3);
                    InsertClassPerk(6, "Replace one [-2] card with one [+0] card", 4);
                    InsertClassPerk(6, "Replace one [+0] card with one [+2] card", 5);
                    InsertClassPerk(6, "Add two [RM] [+1] cards", 6);
                    InsertClassPerk(6, "Add two [RM] [+1] cards", 7);
                    InsertClassPerk(6, "Add two [RM] HEAL [H] 1, self cards", 8);
                    InsertClassPerk(6, "Add two [RM] HEAL [H] 1, self cards", 9);
                    InsertClassPerk(6, "Add one [RM] STUN [ST] card", 10);
                    InsertClassPerk(6, "Add two [RM] [LIGHT] cards", 11);
                    InsertClassPerk(6, "Add two [RM] [LIGHT] cards", 12);
                    InsertClassPerk(6, "Add two [RM] Shield [SH] 1, self cards", 13);
                    InsertClassPerk(6, "Ignore negative item effects and add two [+1] cards", 14);
                    InsertClassPerk(6, "Ignore negative scenario effects", 15);

                    // Valrath Quatermaster
                    InsertClassPerk(7, "Remove two [-1] cards", 1);
                    InsertClassPerk(7, "Remove two [-1] cards", 2);
                    InsertClassPerk(7, "Remove four [+0] cards", 3);
                    InsertClassPerk(7, "Replace one [+0] card with one [+2] card", 4);
                    InsertClassPerk(7, "Replace one [+0] card with one [+2] card", 5);
                    InsertClassPerk(7, "Add two [RM] [+1] cards", 6);
                    InsertClassPerk(7, "Add two [RM] [+1] cards", 7);
                    InsertClassPerk(7, "Add three [RM] MUDDLE [M] cards", 8);
                    InsertClassPerk(7, "Add two [RM] PIERCE [PI] 3 cards", 9);
                    InsertClassPerk(7, "Add one [RM] STUN [ST] card", 10);
                    InsertClassPerk(7, "Add one [RM] ADD TARGET [T] card", 11);
                    InsertClassPerk(7, "Add one [+0] Refresh an item card", 12);
                    InsertClassPerk(7, "Add one [+0] Refresh an item card", 13);
                    InsertClassPerk(7, "Add one [+0] Refresh an item card", 14);
                    InsertClassPerk(7, "Ignore negative item effects and add two [+1] cards", 15);

                    // Aesther Summoner
                    InsertClassPerk(8, "Remove two [-1] cards", 1);
                    InsertClassPerk(8, "Replace one [-2] card with one [+0] card", 2);
                    InsertClassPerk(8, "Replace one [-1] card with one [+1] card", 3);
                    InsertClassPerk(8, "Replace one [-1] card with one [+1] card", 4);
                    InsertClassPerk(8, "Replace one [-1] card with one [+1] card", 5);
                    InsertClassPerk(8, "Add one [+2] card", 6);
                    InsertClassPerk(8, "Add one [+2] card", 7);
                    InsertClassPerk(8, "Add two [RM] WOUND [W] cards", 8);
                    InsertClassPerk(8, "Add two [RM] POISON [PO] cards", 9);
                    InsertClassPerk(8, "Add two [RM] HEAL [H] 1, Self cards", 10);
                    InsertClassPerk(8, "Add two [RM] HEAL [H] 1, Self cards", 11);
                    InsertClassPerk(8, "Add two [RM] HEAL [H] 1, Self cards", 12);
                    InsertClassPerk(8, "Add one [RM] [FIRE] and one [RM] [WIND] card", 13);
                    InsertClassPerk(8, "Add one [RM] [DARK] and one [RM] [EARTH] card", 14);
                    InsertClassPerk(8, "Ignore negative scenario effects and add two [+1] cards", 15);

                    // Aesther Nightshround
                    InsertClassPerk(9, "Remove two [-1] cards", 1);
                    InsertClassPerk(9, "Remove two [-1] cards", 2);
                    InsertClassPerk(9, "Remove four [+0] cards", 3);
                    InsertClassPerk(9, "Add one [-1] [DARK] card", 4);
                    InsertClassPerk(9, "Add one [-1] [DARK] card", 5);
                    InsertClassPerk(9, "Replace one [-1] [DARK] card with one [+1] [DARK] card", 6);
                    InsertClassPerk(9, "Replace one [-1] [DARK] card with one [+1] [DARK] card", 7);
                    InsertClassPerk(9, "Add one [+1] INVISIBLE [IN] cards", 8);
                    InsertClassPerk(9, "Add one [+1] INVISIBLE [IN] cards", 9);
                    InsertClassPerk(9, "Add three [RM] MUDDLE [M] cards", 10);
                    InsertClassPerk(9, "Add three [RM] MUDDLE [M] cards", 11);
                    InsertClassPerk(9, "Add two [RM] HEAL [H] 1, Self cards", 12);
                    InsertClassPerk(9, "Add two [RM] CURSE [C] cards", 13);
                    InsertClassPerk(9, "Add one [RM] ADD TARGET [T] card", 14);
                    InsertClassPerk(9, "Ignore negative scenario effects and add two [+1] cards", 15);

                    // Harrower Plagueherald
                    InsertClassPerk(10, "Replace one [-2] card with one [+0] card", 1);
                    InsertClassPerk(10, "Replace one [-1] card with one [+1] card", 2);
                    InsertClassPerk(10, "Replace one [-1] card with one [+1] card", 3);
                    InsertClassPerk(10, "Replace one [+0] card with one [+2] card", 4);
                    InsertClassPerk(10, "Replace one [+0] card with one [+2] card", 5);
                    InsertClassPerk(10, "Add two [+1] cards", 6);
                    InsertClassPerk(10, "Add one [+1] [WIND] card", 7);
                    InsertClassPerk(10, "Add one [+1] [WIND] card", 8);
                    InsertClassPerk(10, "Add one [+1] [WIND] card", 9);
                    InsertClassPerk(10, "Add three [RM] POISON [PO] cards", 10);
                    InsertClassPerk(10, "Add two [RM] CURSE [C] cards", 11);
                    InsertClassPerk(10, "Add two [RM] IMMOBILIZE [I] cards", 12);
                    InsertClassPerk(10, "Add one [RM] STUN [ST] card", 13);
                    InsertClassPerk(10, "Add one [RM] STUN [ST] card", 14);
                    InsertClassPerk(10, "Ignore negative scenario effects and add one [+1] card", 15);

                    // Inox Berserker
                    InsertClassPerk(11, "Remove two [-1] cards", 1);
                    InsertClassPerk(11, "Remove four [+0] cards", 2);
                    InsertClassPerk(11, "Replace one [-1] card with one [+1] card", 3);
                    InsertClassPerk(11, "Replace one [-1] card with one [+1] card", 4);
                    InsertClassPerk(11, "Replace one [+0] card with one [RM] [+2] card", 5);
                    InsertClassPerk(11, "Replace one [+0] card with one [RM] [+2] card", 6);
                    InsertClassPerk(11, "Add two [RM] WOUND [W] cards", 7);
                    InsertClassPerk(11, "Add two [RM] WOUND [W] cards", 8);
                    InsertClassPerk(11, "Add one [RM] STUN [ST] card", 9);
                    InsertClassPerk(11, "Add one [RM] STUN [ST] card", 10);
                    InsertClassPerk(11, "Add one [RM] [+1] DISARM [D] card", 11);
                    InsertClassPerk(11, "Add two [RM] HEAL [H] 1, SELF cards", 12);
                    InsertClassPerk(11, "Add one [+2] [FIRE] card", 13);
                    InsertClassPerk(11, "Add one [+2] [FIRE] card", 14);
                    InsertClassPerk(11, "Ignore negative item effects", 15);

                    // Quatryl Soothsinger
                    InsertClassPerk(12, "Remove two [-1] cards", 1);
                    InsertClassPerk(12, "Remove two [-1] cards", 2);
                    InsertClassPerk(12, "Remove one [-2] card", 3);
                    InsertClassPerk(12, "Replace two [+1] cards with one [+4] card", 4);
                    InsertClassPerk(12, "Replace two [+1] cards with one [+4] card", 5);
                    InsertClassPerk(12, "Replace one [+0] card with one [+1] IMMOBILIZE [I] card", 6);
                    InsertClassPerk(12, "Replace one [+0] card with one [+1] DISARM [D] card", 7);
                    InsertClassPerk(12, "Replace one [+0] card with one [+2] WOUND [W] card", 8);
                    InsertClassPerk(12, "Replace one [+0] card with one [+2] POISON [PO] card", 9);
                    InsertClassPerk(12, "Replace one [+0] card with one [+2] CURSE [C] card", 10);
                    InsertClassPerk(12, "Replace one [+0] card with one [+3] MUDDLE [M] card", 11);
                    InsertClassPerk(12, "Replace one [-1] card with one [+0] STUN [ST] card", 12);
                    InsertClassPerk(12, "Add three [RM] [+1] cards", 13);
                    InsertClassPerk(12, "Add two [RM] CURSE [C] cards", 14);
                    InsertClassPerk(12, "Add two [RM] CURSE [C] cards", 15);

                    // Orchid Doomstalker
                    InsertClassPerk(13, "Remove two [-1] cards", 1);
                    InsertClassPerk(13, "Remove two [-1] cards", 2);
                    InsertClassPerk(13, "Replace two [+0] cards with two [+1] cards", 3);
                    InsertClassPerk(13, "Replace two [+0] cards with two [+1] cards", 4);
                    InsertClassPerk(13, "Replace two [+0] cards with two [+1] cards", 5);
                    InsertClassPerk(13, "Add two [RM] [+1] cards", 6);
                    InsertClassPerk(13, "Add two [RM] [+1] cards", 7);
                    InsertClassPerk(13, "Add one [+2] MUDDLE [M] card", 8);
                    InsertClassPerk(13, "Add one [+1] POISON [PO] card", 9);
                    InsertClassPerk(13, "Add one [+1] WOUND [W] card", 10);
                    InsertClassPerk(13, "Add one [+1] IMMOBILIZE [I] card", 11);
                    InsertClassPerk(13, "Add one [+0] STUN [ST] card", 12);
                    InsertClassPerk(13, "Add one [RM] ADD TARGET [T] card", 13);
                    InsertClassPerk(13, "Add one [RM] ADD TARGET [T] card", 14);
                    InsertClassPerk(13, "Ignore negative scenario effects", 15);

                    // Human Sawbones
                    InsertClassPerk(14, "Remove two [-1] cards", 1);
                    InsertClassPerk(14, "Remove two [-1] cards", 2);
                    InsertClassPerk(14, "Remove four [+0] cards", 3);
                    InsertClassPerk(14, "Replace one [+0] card with one [+2] card", 4);
                    InsertClassPerk(14, "Replace one [+0] card with one [+2] card", 5);
                    InsertClassPerk(14, "Add one [RM] [+2] card", 6);
                    InsertClassPerk(14, "Add one [RM] [+2] card", 7);
                    InsertClassPerk(14, "Add one [+1] IMMOBILIZE [I] card", 8);
                    InsertClassPerk(14, "Add one [+1] IMMOBILIZE [I] card", 9);
                    InsertClassPerk(14, "Add two [RM] WOUND [W] cards", 10);
                    InsertClassPerk(14, "Add two [RM] WOUND [W] cards", 11);
                    InsertClassPerk(14, "Add one [RM] STUN [ST] card", 12);
                    InsertClassPerk(14, "Add one [RM] HEAL [H] 3, SELF card", 13);
                    InsertClassPerk(14, "Add one [RM] HEAL [H] 3, SELF card", 14);
                    InsertClassPerk(14, "Add one [+0] Refresh an item card", 15);

                    // Savvas Elementalist
                    InsertClassPerk(15, "Remove two [-1] cards", 1);
                    InsertClassPerk(15, "Remove two [-1] cards", 2);
                    InsertClassPerk(15, "Replace one [-1] card with one [+1] card", 3);
                    InsertClassPerk(15, "Replace one [+0] card with one [+2] card", 4);
                    InsertClassPerk(15, "Replace one [+0] card with one [+2] card", 5);
                    InsertClassPerk(15, "Add three [+0] [FIRE] cards", 6);
                    InsertClassPerk(15, "Add three [+0] [FROST] cards", 7);
                    InsertClassPerk(15, "Add three [+0] [WIND] cards", 8);
                    InsertClassPerk(15, "Add three [+0] [EARTH] cards", 9);
                    InsertClassPerk(15, "Replace two [+0] cards with one [+0] [FIRE] and one [+0] [EARTH] card", 10);
                    InsertClassPerk(15, "Replace two [+0] cards with one [+0] [FROST] and one [+0] [WIND] card", 11);
                    InsertClassPerk(15, "Add two [+1] PUSH [PU] 1 cards", 12);
                    InsertClassPerk(15, "Add one [+1] WOUND [W] card", 13);
                    InsertClassPerk(15, "Add one [+0] STUN [ST] card", 14);
                    InsertClassPerk(15, "Add one [+0] ADD TARGET [T] card", 15);

                    // Vermling Beasttyrant
                    InsertClassPerk(16, "Remove two [-1] cards", 1);
                    InsertClassPerk(16, "Replace one [-1] card with one [+1] card", 2);
                    InsertClassPerk(16, "Replace one [-1] card with one [+1] card", 3);
                    InsertClassPerk(16, "Replace one [-1] card with one [+1] card", 4);
                    InsertClassPerk(16, "Replace one [+0] card with one [+2] card", 5);
                    InsertClassPerk(16, "Replace one [+0] card with one [+2] card", 6);
                    InsertClassPerk(16, "Add one [+1] WOUND [W] card", 7);
                    InsertClassPerk(16, "Add one [+1] WOUND [W] card", 8);
                    InsertClassPerk(16, "Add one [+1] IMMOBILIZE [I] card", 9);
                    InsertClassPerk(16, "Add one [+1] IMMOBILIZE [I] card", 10);
                    InsertClassPerk(16, "Add two [RM] HEAL [H] 1, Self cards", 11);
                    InsertClassPerk(16, "Add two [RM] HEAL [H] 1, Self cards", 12);
                    InsertClassPerk(16, "Add two [RM] HEAL [H] 1, Self cards", 13);
                    InsertClassPerk(16, "Add two [RM] [EARTH] cards", 14);
                    InsertClassPerk(16, "Ignore negative scenario effects", 15);

                    // Harrower Bladeswarm
                    InsertClassPerk(17, "Remove one [-2] card", 1);
                    InsertClassPerk(17, "Remove four [+0] cards", 2);
                    InsertClassPerk(17, "Replace one [-1] card with one [+1][WIND] card", 3);
                    InsertClassPerk(17, "Replace one [-1] card with one [+1][EARTH] card", 4);
                    InsertClassPerk(17, "Replace one [-1] card with one [+1][LIGHT] card", 5);
                    InsertClassPerk(17, "Replace one [-1] card with one [+1][DARK] card", 6);
                    InsertClassPerk(17, "Add two [RM] Heal [H]1 cards", 7);
                    InsertClassPerk(17, "Add two [RM] Heal [H]1 cards", 8);
                    InsertClassPerk(17, "Add one [+1] WOUND [W] card", 9);
                    InsertClassPerk(17, "Add one [+1] WOUND [W] card", 10);
                    InsertClassPerk(17, "Add one [+1] POISON [PO] card", 11);
                    InsertClassPerk(17, "Add one [+1] POISON [PO] card", 12);
                    InsertClassPerk(17, "Add one [+2] MUDDLE [M] card", 13);
                    InsertClassPerk(17, "Ignore negative item effects and add one [+1] card", 14);
                    InsertClassPerk(17, "Ignore negative scenario effects and add one [+1] card", 15);


                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }
        }

        internal static void InsertClassPerk(int classId, string perktext, int checkboxnumber)
        {
            var item = new DL_ClassPerk
            {
                ClassId = classId,
                Perktext = perktext,
                Checkboxnumber = checkboxnumber
            };

            ClassPerkRepository.InsertOrReplace(item);
        }

        public static void UpdateMissingItems()
        {
            FillItems();
        }

        private static void FillItems()
        {
            var items = ItemRepository.Get();

            Connection.BeginTransaction();
            try
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

                // Prosperity
                
                SaveItem(itemnumber: 1, itemname: "Boots of Striding", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 1, items);
                SaveItem(itemnumber: 2, itemname: "Winged Shoes", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 1, items);
                SaveItem(itemnumber: 3, itemname: "Hide Armor", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 10, prosperitylevel: 1, items);
                SaveItem(itemnumber: 4, itemname: "Leather Armor", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 1, items);
                SaveItem(itemnumber: 5, itemname: "Cloak of Invisibility", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 1, items);
                SaveItem(itemnumber: 6, itemname: "Eagle-Eye Goggles", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 1, items);
                SaveItem(itemnumber: 7, itemname: "Iron Helmet", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 10, prosperitylevel: 1, items);
                SaveItem(itemnumber: 8, itemname: "Heater Shield", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 1, items);
                SaveItem(itemnumber: 9, itemname: "Piercing Bow", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 1, items);
                SaveItem(itemnumber: 10, itemname: "War Hammer", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 1, items);
                SaveItem(itemnumber: 11, itemname: "Poison Dagger", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 1, items);
                SaveItem(itemnumber: 12, itemname: "Minor Healing Potion", itemcategorie: 5, itemcount: 4, itemtext: "", itemprice: 10, prosperitylevel: 1, items);
                SaveItem(itemnumber: 13, itemname: "Minor Stamina Potion", itemcategorie: 5, itemcount: 4, itemtext: "", itemprice: 10, prosperitylevel: 1, items);
                SaveItem(itemnumber: 14, itemname: "Minor Power Potion", itemcategorie: 5, itemcount: 4, itemtext: "", itemprice: 10, prosperitylevel: 1, items);

                SaveItem(itemnumber: 15, itemname: "Boots of Speed", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 2, items);
                SaveItem(itemnumber: 16, itemname: "Cloak of Pockets", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 2, items);
                SaveItem(itemnumber: 17, itemname: "Empowering Talisman", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 45, prosperitylevel: 2, items);
                SaveItem(itemnumber: 18, itemname: "Battle-Axe", itemcategorie: 2, itemcount: 18, itemtext: "", itemprice: 20, prosperitylevel: 2, items);
                SaveItem(itemnumber: 19, itemname: "Weighted Net", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 2, items);
                SaveItem(itemnumber: 20, itemname: "Minor Mana Potion", itemcategorie: 5, itemcount: 4, itemtext: "", itemprice: 10, prosperitylevel: 2, items);
                SaveItem(itemnumber: 21, itemname: "Stun Powder", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 2, items);

                SaveItem(itemnumber: 22, itemname: "Heavy Greaves", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 3, items);
                SaveItem(itemnumber: 23, itemname: "Chainmail", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 3, items);
                SaveItem(itemnumber: 24, itemname: "Amulet of Life", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 3, items);
                SaveItem(itemnumber: 25, itemname: "Jagged Sword", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 3, items);
                SaveItem(itemnumber: 26, itemname: "Long Spear", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 3, items);
                SaveItem(itemnumber: 27, itemname: "Major Healing Potion", itemcategorie: 5, itemcount: 4, itemtext: "", itemprice: 30, prosperitylevel: 3, items);
                SaveItem(itemnumber: 28, itemname: "Moon Earring", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 3, items);

                SaveItem(itemnumber: 29, itemname: "Comfortable Shoes", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 4, items);
                SaveItem(itemnumber: 30, itemname: "Studded Leather", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 4, items);
                SaveItem(itemnumber: 31, itemname: "Hawk Helm", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 4, items);
                SaveItem(itemnumber: 32, itemname: "Tower Shield", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 4, items);
                SaveItem(itemnumber: 33, itemname: "Volatile Bomb", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 4, items);
                SaveItem(itemnumber: 34, itemname: "Major Stamina Potion", itemcategorie: 5, itemcount: 4, itemtext: "", itemprice: 30, prosperitylevel: 4, items);
                SaveItem(itemnumber: 35, itemname: "Falcon Figurine", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 4, items);

                SaveItem(itemnumber: 36, itemname: "Boots of Dashing", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 5, items);
                SaveItem(itemnumber: 37, itemname: "Robes of Evocation", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 5, items);
                SaveItem(itemnumber: 38, itemname: "Heavy Basinet", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 5, items);
                SaveItem(itemnumber: 39, itemname: "Hooked Chain", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 5, items);
                SaveItem(itemnumber: 40, itemname: "Versatile Dagger", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 25, prosperitylevel: 5, items);
                SaveItem(itemnumber: 41, itemname: "Major Power Potion", itemcategorie: 5, itemcount: 4, itemtext: "", itemprice: 40, prosperitylevel: 5, items);
                SaveItem(itemnumber: 42, itemname: "Ring of Haste", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 5, items);

                SaveItem(itemnumber: 43, itemname: "Boots of Quickness", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 75, prosperitylevel: 6, items);
                SaveItem(itemnumber: 44, itemname: "Splintmail", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 35, prosperitylevel: 6, items);
                SaveItem(itemnumber: 45, itemname: "Pendant of Dark Pacts", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 75, prosperitylevel: 6, items);
                SaveItem(itemnumber: 46, itemname: "Spiked Shield", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 6, items);
                SaveItem(itemnumber: 47, itemname: "Reaping Scythe", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 6, items);
                SaveItem(itemnumber: 48, itemname: "Major Mana Potion", itemcategorie: 5, itemcount: 4, itemtext: "", itemprice: 30, prosperitylevel: 6, items);
                SaveItem(itemnumber: 49, itemname: "Sun Earring", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 6, items);

                SaveItem(itemnumber: 50, itemname: "Steel Sabatons", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 60, prosperitylevel: 7, items);
                SaveItem(itemnumber: 51, itemname: "Shadow Armor", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 7, items);
                SaveItem(itemnumber: 52, itemname: "Protective Charm", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 60, prosperitylevel: 7, items);
                SaveItem(itemnumber: 53, itemname: "Black Knife", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 25, prosperitylevel: 7, items);
                SaveItem(itemnumber: 54, itemname: "Staff of Eminence", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 60, prosperitylevel: 7, items);
                SaveItem(itemnumber: 55, itemname: "Super Healing Potion", itemcategorie: 5, itemcount: 4, itemtext: "", itemprice: 50, prosperitylevel: 7, items);
                SaveItem(itemnumber: 56, itemname: "Ring of Brutality", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 7, items);

                SaveItem(itemnumber: 57, itemname: "Serene Sandals", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 75, prosperitylevel: 8, items);
                SaveItem(itemnumber: 58, itemname: "Cloak of Phasing", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 75, prosperitylevel: 8, items);
                SaveItem(itemnumber: 59, itemname: "Telescopic Lens", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 8, items);
                SaveItem(itemnumber: 60, itemname: "Unstable Explosives", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 45, prosperitylevel: 8, items);
                SaveItem(itemnumber: 61, itemname: "Wall Shield", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 60, prosperitylevel: 8, items);
                SaveItem(itemnumber: 62, itemname: "Doom Powder", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 8, items);
                SaveItem(itemnumber: 63, itemname: "Luckey Eye", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 60, prosperitylevel: 8, items);

                SaveItem(itemnumber: 64, itemname: "Boots of Sprinting", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 75, prosperitylevel: 9, items);
                SaveItem(itemnumber: 65, itemname: "Platemail", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 9, items);
                SaveItem(itemnumber: 66, itemname: "Mask of Terror", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 9, items);
                SaveItem(itemnumber: 67, itemname: "Balanced Blade", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 60, prosperitylevel: 9, items);
                SaveItem(itemnumber: 68, itemname: "Halberd", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 75, prosperitylevel: 9, items);
                SaveItem(itemnumber: 69, itemname: "Star Earring", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 70, prosperitylevel: 9, items);
                SaveItem(itemnumber: 70, itemname: "Second Chance Ring", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 75, prosperitylevel: 9, items);

                // Designs
                SaveItem(itemnumber: 71, itemname: "Boots of Levitation", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 100, items);
                SaveItem(itemnumber: 72, itemname: "Shoes of Happiness", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 100, items);
                SaveItem(itemnumber: 73, itemname: "Blinking Cape", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 100, items);
                SaveItem(itemnumber: 74, itemname: "Swordedge Armor", itemcategorie: 1, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 100, items);
                SaveItem(itemnumber: 75, itemname: "Circle of Elements", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 25, prosperitylevel: 100, items);
                SaveItem(itemnumber: 76, itemname: "Chain Hood", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 100, items);
                SaveItem(itemnumber: 77, itemname: "Frigid Blade", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 78, itemname: "Storm Blade", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 79, itemname: "Inferno Blade", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 80, itemname: "Tremor Blade", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 81, itemname: "Brilliant Blade", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 82, itemname: "Night Blade", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 83, itemname: "Wand of Frost", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 84, itemname: "Wand of Storms", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 85, itemname: "Wand of Infernos", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 86, itemname: "Wand of Tremors", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 87, itemname: "Wand of Brilliance", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 88, itemname: "Wand of Darkness", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 89, itemname: "Minure Cure Potion", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 10, prosperitylevel: 100, items);
                SaveItem(itemnumber: 90, itemname: "Major Cure Potion", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 91, itemname: "Steel Ring", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 100, items);
                SaveItem(itemnumber: 92, itemname: "Dampening Ring", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 25, prosperitylevel: 100, items);
                SaveItem(itemnumber: 93, itemname: "Scroll of Power", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 94, itemname: "Scroll of Healing", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 100, items);
                SaveItem(itemnumber: 95, itemname: "Scroll of Stamina", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 100, items);

                // Items
                SaveItem(itemnumber: 96, itemname: "Rocket Boots", itemcategorie: 4, itemcount: 2, itemtext: "", itemprice: 80, prosperitylevel: 150, items);
                SaveItem(itemnumber: 97, itemname: "Endurance Footwraps", itemcategorie: 4, itemcount: 1, itemtext: "", itemprice: 40, prosperitylevel: 150, items);
                SaveItem(itemnumber: 98, itemname: "Drakescale Boots", itemcategorie: 4, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 99, itemname: "Magma Waders", itemcategorie: 4, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 100, itemname: "Robes of Summoning", itemcategorie: 1, itemcount: 1, itemtext: "", itemprice: 40, prosperitylevel: 150, items);
                SaveItem(itemnumber: 101, itemname: "Second Skin", itemcategorie: 1, itemcount: 1, itemtext: "", itemprice: 30, prosperitylevel: 150, items);
                SaveItem(itemnumber: 102, itemname: "Sacrificial Robes", itemcategorie: 1, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 103, itemname: "Drakescale Armor", itemcategorie: 1, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 104, itemname: "Steam Armor", itemcategorie: 1, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 105, itemname: "Flea-Bitten Shawl", itemcategorie: 1, itemcount: 1, itemtext: "", itemprice: 10, prosperitylevel: 150, items);
                SaveItem(itemnumber: 106, itemname: "Necklace of Teeth", itemcategorie: 0, itemcount: 1, itemtext: "", itemprice: 40, prosperitylevel: 150, items);
                SaveItem(itemnumber: 107, itemname: "Horned Helm", itemcategorie: 0, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 150, items);
                SaveItem(itemnumber: 108, itemname: "Drakescale Helm", itemcategorie: 0, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 109, itemname: "Thief's Hood", itemcategorie: 0, itemcount: 1, itemtext: "", itemprice: 20, prosperitylevel: 150, items);
                SaveItem(itemnumber: 110, itemname: "Helm of the Mountain", itemcategorie: 0, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 111, itemname: "Wave Crest", itemcategorie: 0, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 112, itemname: "Ancient Drill", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 30, prosperitylevel: 150, items);
                SaveItem(itemnumber: 113, itemname: "Skullbane Axe", itemcategorie: 3, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 114, itemname: "Staff of Xorn", itemcategorie: 3, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 115, itemname: "Mountain Hammer", itemcategorie: 3, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 116, itemname: "Fueled Falchion", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 20, prosperitylevel: 150, items);
                SaveItem(itemnumber: 117, itemname: "Bloody Axe", itemcategorie: 2, itemcount: 2, itemtext: "", itemprice: 40, prosperitylevel: 150, items);
                SaveItem(itemnumber: 118, itemname: "Staff of Elements", itemcategorie: 3, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 119, itemname: "Skull of Hatred", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 120, itemname: "Staff of Summoning", itemcategorie: 3, itemcount: 1, itemtext: "", itemprice: 60, prosperitylevel: 150, items);
                SaveItem(itemnumber: 121, itemname: "Orb of Dawn", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 122, itemname: "Orb of Twilight", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 123, itemname: "Ring of Skulls", itemcategorie: 5, itemcount: 2, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 124, itemname: "Doomed Compass", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 125, itemname: "Curious Gear", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 126, itemname: "Remote Spider", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 40, prosperitylevel: 150, items);
                SaveItem(itemnumber: 127, itemname: "GiantRemote Spider", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 60, prosperitylevel: 150, items);
                SaveItem(itemnumber: 128, itemname: "Black Censer", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 129, itemname: "Black Card", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 75, prosperitylevel: 150, items);
                SaveItem(itemnumber: 130, itemname: "Helix Ring", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 150, items);
                SaveItem(itemnumber: 131, itemname: "Heart of the Betrayer", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 60, prosperitylevel: 150, items);
                SaveItem(itemnumber: 132, itemname: "Power Core", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 75, prosperitylevel: 150, items);
                SaveItem(itemnumber: 133, itemname: "Resonant Crystal", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 20, prosperitylevel: 150, items);

                // Soloscenario rewards
                SaveItem(itemnumber: 134, itemname: "Imposing Blade", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 135, itemname: "Focusing Ray", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 136, itemname: "Volatile Elixir", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 137, itemname: "Silent Stiletto", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 138, itemname: "Stone Charm", itemcategorie: 0, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 139, itemname: "Psychic Knife", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 140, itemname: "Sun Shield", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 141, itemname: "Utility Belt", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 142, itemname: "Phasing Idol", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 143, itemname: "Smoke Elixir", itemcategorie: 5, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 144, itemname: "Pendant of the Plague", itemcategorie: 0, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 145, itemname: "Mask of Death", itemcategorie: 0, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 146, itemname: "Master's Lute", itemcategorie: 3, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 147, itemname: "Cloak of the Hunter", itemcategorie: 1, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 148, itemname: "Doctor's Coat", itemcategorie: 1, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 149, itemname: "Elemental Boots", itemcategorie: 4, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 150, itemname: "Staff of Command", itemcategorie: 3, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);
                SaveItem(itemnumber: 151, itemname: "Blade of the Sands", itemcategorie: 2, itemcount: 1, itemtext: "", itemprice: 50, prosperitylevel: 200, items);

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void SaveItem(int itemnumber, string itemname, int itemcategorie, int itemcount, string itemtext, int itemprice, int prosperitylevel, List<DL_Item> items)
        {
            if (!items.Any(x=> x.Itemnumber == itemnumber))
            {
                var item = new DL_Item
                {
                    Itemcategorie = itemcategorie,
                    Itemcount = itemcount,
                    Itemname = itemname,
                    Itemnumber = itemnumber,
                    Itemprice = itemprice,
                    Itemtext = itemtext,
                    Prosperitylevel = prosperitylevel
                };

                ItemRepository.InsertOrReplace(item);

                items.Add(item);
            }           
        }

        private static void FillPersonalQuests()
        {
            if (!PersonalQuestRepository.Get().Any())
            {
                Connection.BeginTransaction();
                try
                {
                    SavePersonalQuest(
                        510,
                        "Seeker of Xorn",
                        "Complete three \"Crypt\" scenarios. Then unlock \"Noxious Cellar\" (Scenario 52) and follow it to a conclusion.",
                        "class",
                        11);
                    SavePersonalQuest(
                        511,
                        "Merchant Class",
                        "Own two Head items, two armor items, two boot items, three one hand or two hand items, and four small items.",
                        "class",
                        8);
                    SavePersonalQuest(
                        512,
                        "Greed Is Good",
                        "Have 200 gold in your possession",
                        "class",
                        8);
                    SavePersonalQuest(
                        513,
                        "Finding the Cure",
                        "Kill eight Forest Imps. Then unlock \"Forgotten Grove\" (Scenario 59) and follow it to a conclusion.",
                        "Envelope X",
                        0);
                    SavePersonalQuest(
                        514,
                        "A Study of Anatomy",
                        "Experience your party members becoming exhausted fifteen times",
                        "class",
                        15);
                    SavePersonalQuest(
                        515,
                        "Law Bringer",
                        "Kill twenty Bandits or Cultists",
                        "class",
                        7);
                    SavePersonalQuest(
                        516,
                        "Pounds of Flesh",
                        "Kill fifteen Vermlings",
                        "class",
                        12);
                    SavePersonalQuest(
                        517,
                        "Trophy Hunt",
                        "Kill twenty different types of monsters",
                        "class",
                        17);
                    SavePersonalQuest(
                        518,
                        "Eternal Wanderer",
                        "Complete fifteen different scenarios",
                        "class",
                        9);
                    SavePersonalQuest(
                        519,
                        "Battle Legend",
                        "Earn fifteen checkmarks from completed battle goals",
                        "class",
                        13);
                    SavePersonalQuest(
                        520,
                        "Implement of Light",
                        "Find the Skullbane Axe in the Necromancer's Sanctum and then use it to kill seven Living Bones, Living Corpses, or Living Spirits",
                        "class",
                        7);
                    SavePersonalQuest(
                        521,
                        "Take Back the Trees",
                        "Complete three scenarios in the Dagger Forest. Then unlock \"Foggy Thicket\" (Scenario 55) and follow it to a conclusion.",
                        "class",
                        14);
                    SavePersonalQuest(
                        522,
                        "The Thin Places",
                        "Complete six side scenarios (scenario number > 51).",
                        "class",
                        10);
                    SavePersonalQuest(
                        523,
                        "Aberrant Slayer",
                        "Kill one Flame Demon, one Frost Demon, one Wind Demon, one Earth Demon, one Night Demon, and one Sun Demon.",
                        "class",
                        16);
                    SavePersonalQuest(
                        524,
                        "Fearless Stand",
                        "Kill twenty elite monsters",
                        "class",
                        17);
                    SavePersonalQuest(
                        525,
                        "Piety in All Things",
                        "Donate 120 gold to the Sanctuary of the Great Oak.",
                        "class",
                        15);
                    SavePersonalQuest(
                        526,
                        "Vengeance",
                        "Complete four scenarios in Gloomhaven. Then unlock \"Investigation\" (Scenario 57) and follow it to a conclusion.",
                        "Envelope X",
                        0);
                    SavePersonalQuest(
                        527,
                        "Zealot of the Blood God",
                        "Become exhausted twelve times.",
                        "class",
                        12);
                    SavePersonalQuest(
                        528,
                        "Goliath Toppler",
                        "Complete four boss scenarios.",
                        "class",
                        14);
                    SavePersonalQuest(
                        529,
                        "The Fall of Man",
                        "Complete two scenarios in the Lingering Swamp. Then unlock \"Fading Lighthouse\" (Scenario 61) and follow it to a conclusion.",
                        "class",
                        10);
                    SavePersonalQuest(
                        530,
                        "Augmented Abilities",
                        "Purchase four enhancements (requires \"The Power of Enhancement\" global achievement.)",
                        "class",
                        9);
                    SavePersonalQuest(
                        531,
                        "Elemental Samples",
                        "Complete a scenario in each of the following areas: Gloomhaven, Dagger Forest, Lingering Swamp, Watcher Mountains, Copperneck Mountains, and Misty Sea.",
                        "class",
                        16);
                    SavePersonalQuest(
                        532,
                        "A Helping Hand",
                        "Experience two other characters achieving their personal quest.",
                        "class",
                        13);
                    SavePersonalQuest(
                        533,
                        "The Perfect Poison",
                        "Kill three Oozes, three Lurkers, and three Spitting Drakes.",
                        "class",
                        11);

                    Connection.Commit();
                }
                catch
                {
                    Connection.Rollback();
                    throw;
                }
            }

        }

        private static void SavePersonalQuest(int questnumber, string questname, string questgoal, string questreward, int questrewardclassid)
        {
            var personalquest = new DL_PersonalQuest
            {
                QuestNumber = questnumber,
                QuestName = questname,
                QuestGoal = questgoal,
                QuestReward = questreward,
                QuestRewardClassId = questrewardclassid
            };

            PersonalQuestRepository.InsertOrReplace(personalquest);
        }

        private static void FillPartyAchievements()
        {
            Connection.BeginTransaction();
            try
            {
                Connection.DropTable<DL_PartyAchievement>();
                Connection.CreateTable<DL_PartyAchievement>();

                SavePartyAchievement(
                    1,
                    "A Demon's Errand"
                    );
                SavePartyAchievement(
                    2,
                    "A Map to Treasure"
                    );
                SavePartyAchievement(
                    3,
                    "Across the Divide"
                    );
                SavePartyAchievement(
                    4,
                    "An Invitation"
                    );
                SavePartyAchievement(
                    5,
                    "Bad Business"
                    );
                SavePartyAchievement(
                    6,
                    "Dark Bounty"
                    );
                SavePartyAchievement(
                    7,
                    "Debt Collection"
                    );
                SavePartyAchievement(
                    8,
                    "First Steps"
                    );
                SavePartyAchievement(
                    9,
                    "Fish's Aid"
                    );
                SavePartyAchievement(
                    10,
                    "Following Clues"
                    );
                SavePartyAchievement(
                    11,
                    "Grave Job"
                    );
                SavePartyAchievement(
                    12,
                    "High Sea Escort"
                    );
                SavePartyAchievement(
                    13,
                     "Jekserah's Plans"
                     );
                SavePartyAchievement(
                    14,
                    "Redthorn's Aid"
                    );
                SavePartyAchievement(
                    15,
                     "Sin-Ra"
                     );
                SavePartyAchievement(
                    16,
                    "Stonebreaker's Censer"
                    );
                SavePartyAchievement(
                    17,
                    "The Drake's Command"
                     );
                SavePartyAchievement(
                    18,
                    "The Drake's Treasure"
                    );
                SavePartyAchievement(
                    19,
                     "The Poison's Source"
                     );
                SavePartyAchievement(
                    20,
                    "The Scepter and the Voice"
                    );
                SavePartyAchievement(
                    21,
                     "The Voice's Command"
                     );
                SavePartyAchievement(
                    22,
                    "The Voice's Treasure"
                    );
                SavePartyAchievement(
                    23,
                     "Through the Nest"
                     );
                SavePartyAchievement(
                    24,
                    "Through the Ruins"
                    );
                SavePartyAchievement(
                    25,
                    "Through the Trench"
                    );
                SavePartyAchievement(
                    26,
                    "Tremors"
                    );
                SavePartyAchievement(
                    27,
                    "Water Staff"
                    );
                SavePartyAchievement(
                    28,
                    "Sun Blessed"
                    );

                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        internal static void SavePartyAchievement(int number, string name)
        {
            var achievement = new DL_PartyAchievement
            {
                Name = name,
                InternalNumber = number
            };

            PartyAchievementRepository.InsertOrReplace(achievement);
        }

        private static void FillAchievements()
        {
            Connection.BeginTransaction();
            try
            {
                Connection.DropTable<DL_Achievement>();
                Connection.CreateTable<DL_Achievement>();

                Connection.DropTable<DL_AchievementType>();
                Connection.CreateTable<DL_AchievementType>();

                var achievements = new List<DL_Achievement>
                {
                    new DL_Achievement
                    {
                        Name = "Slain",
                        InternalNumber = 1
                    },
                    new DL_Achievement
                    {
                        Name = "Aided",
                        InternalNumber = 2
                    }
                };

                SaveAchievementType("The Drake", 100, achievements: achievements);

                achievements = new List<DL_Achievement>
                {
                    new DL_Achievement
                    {
                        Name = "Militaristic",
                        InternalNumber = 1
                    },
                    new DL_Achievement
                    {
                        Name = "Economic",
                        InternalNumber = 2
                    },
                    new DL_Achievement
                    {
                        Name = "Demonic",
                        InternalNumber = 3
                    }
                };

                SaveAchievementType("City Rule", 200, achievements: achievements);

                achievements = new List<DL_Achievement>
                {
                    new DL_Achievement
                    {
                        Name = "Recovered",
                        InternalNumber = 1
                    },
                    new DL_Achievement
                    {
                        Name = "Lost",
                        InternalNumber = 2
                    },
                    new DL_Achievement
                    {
                        Name = "Cleansed",
                        InternalNumber = 3
                    }
                };

                SaveAchievementType("Artifact", 300, achievements: achievements);

                achievements = new List<DL_Achievement>
                {
                    new DL_Achievement
                    {
                        Name = "Silenced",
                        InternalNumber = 1
                    },
                    new DL_Achievement
                    {
                        Name = "Freed",
                        InternalNumber = 2
                    }
                };

                SaveAchievementType("The Voice", 400, achievements: achievements);
                SaveAchievementType("The Merchant flees", 5);
                SaveAchievementType("The Dead Invade", 6);
                SaveAchievementType("The Edge of Darkness", 7);
                SaveAchievementType("The Power of Enhancement", 8);
                SaveAchievementType("Water-Breathing", 9);
                SaveAchievementType("The Demon Dethroned", 10);
                SaveAchievementType("The Rift Neutralized", 11); // The Rift Closed
                SaveAchievementType("End of the Invasion", 12);
                SaveAchievementType("End of Corruption", 130, 3);
                SaveAchievementType("End of Gloom", 14);
                SaveAchievementType("Ancient Technology", 150, 5);
                SaveAchievementType("Annihilation of the Order", 16);
                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void SaveAchievementType(string name, int internalNumber, int steps = 1, List<DL_Achievement> achievements = null)
        {
            if (achievements == null)
            {
                achievements = new List<DL_Achievement>();
            }

            var achievement = new DL_AchievementType
            {
                Name = name,
                InternalNumber = internalNumber,
                Steps = steps,
                Achievements = achievements
            };

            AchievementTypeRepository.InsertOrReplace(achievement);
        }

        private static void FillScenarios()
        {
            Connection.BeginTransaction();
            try
            {
                Connection.DropTable<DL_Scenario>();
                Connection.CreateTable<DL_Scenario>();

                SaveScenario("Black Barrow", 1, "2");
                SaveScenario("Barrow Lair", 2, "3,4", requiredPartyAchievements: "8");
                SaveScenario("Inox Encampment", 3, "8,9", blockingGlobalAchievements: "5");
                SaveScenario("Crypt of the Damned", 4, "5,6");
                SaveScenario("Ruinous Crypt", 5, "10,19,14");
                SaveScenario("Decaying Crypt", 6, "8");
                SaveScenario("Vibrant Grotto", 7, "20", requiredGlobalAchievements: "5,8");
                SaveScenario("Gloomhaven Warehouse", 8, "14,13,7", requiredPartyAchievements: "13", blockingGlobalAchievements: "6");
                SaveScenario("Diamond Mine", 9, "11,12", blockingGlobalAchievements: "5");
                SaveScenario("Plane of Elemental Power", 10, "21,22", blockingGlobalAchievements: "11");
                SaveScenario("Square (a)", 11, "16,18", blockingGlobalAchievements: "12");
                SaveScenario("Square (b)", 12, "16,18,28", blockingGlobalAchievements: "12");
                SaveScenario("Temple of the Seer", 13, "15,17,20");
                SaveScenario("Frozen Hollow", 14);
                SaveScenario("Shrine of Strength", 15);
                SaveScenario("Mountain Pass", 16, "24,25");
                SaveScenario("Lost Island", 17);
                SaveScenario("Abandoned Sewers", 18, "23,43,26,14");
                SaveScenario("Forgotten Crypt", 19, "27", requiredGlobalAchievements: "8");
                SaveScenario("Necromancer's Sanctum", 20, "18,16,28", requiredGlobalAchievements: "5");
                SaveScenario("Infernal Throne", 21, blockingGlobalAchievements: "11");
                SaveScenario("Temple of the Elements", 22, "36,35,31", requiredPartyAchievements: "1,10"); // OR
                SaveScenario("Deep Ruins", 23);
                SaveScenario("Echo Chamber", 24, "30,32");
                SaveScenario("Icecrag Ascent", 25, "33,34");
                SaveScenario("Ancient Cistern", 26, "22", requiredGlobalAchievements: "9", requiredPartyAchievements: "24"); //OR
                SaveScenario("Ruinous Rift", 27, blockingGlobalAchievements: "302", requiredPartyAchievements: "16");
                SaveScenario("Outer Ritual Chamber", 28, "29", requiredPartyAchievements: "6");
                SaveScenario("Sanctuary of Gloom", 29, requiredPartyAchievements: "4");
                SaveScenario("Shrine of the Depths", 30, "42", requiredPartyAchievements: "21");
                SaveScenario("Plane of Night", 31, "39,37,38,43", requiredGlobalAchievements: "8,301");
                SaveScenario("Decrepit Wood", 32, "33,40", requiredPartyAchievements: "21");
                SaveScenario("Savvas Armory", 33, requiredPartyAchievements: "21,17"); // OR
                SaveScenario("Scorched Summit", 34, requiredPartyAchievements: "17", blockingGlobalAchievements: "102");
                SaveScenario("Battlements (a)", 35, "45", requiredPartyAchievements: "1", blockingGlobalAchievements: "11");
                SaveScenario("Battlements (b)", 36, requiredPartyAchievements: "1", blockingGlobalAchievements: "11");
                SaveScenario("Doom Trench", 37, "47", requiredGlobalAchievements: "9");
                SaveScenario("Slave Pens", 38, "48,44");
                SaveScenario("Treacherous Divide", 39, "46,15");
                SaveScenario("Ancient Defense Network", 40, "41", requiredPartyAchievements: "21,22");
                SaveScenario("Timeworn Tomb", 41, requiredPartyAchievements: "21");
                SaveScenario("Realm of the Voice", 42, requiredPartyAchievements: "20", blockingGlobalAchievements: "402");
                SaveScenario("Drake Nest", 43, requiredGlobalAchievements: "8");
                SaveScenario("Tribal Assault", 44, requiredPartyAchievements: "14");
                SaveScenario("Rebel Swamp", 45, "49,50", requiredGlobalAchievements: "203");
                SaveScenario("Nigthmare Peak", 46, "51", requiredPartyAchievements: "3");
                SaveScenario("Lair of the Unseeing Eye", 47, "51", requiredPartyAchievements: "25");
                SaveScenario("Shadow Weald", 48, "51", requiredPartyAchievements: "14");
                SaveScenario("Rebel's Stand", 49, requiredGlobalAchievements: "203");
                SaveScenario("Ghost Fortress", 50, requiredGlobalAchievements: "203", blockingGlobalAchievements: "16");
                SaveScenario("The Void", 51, requiredGlobalAchievements: "133");
                SaveScenario("Noxious Cellar", 52, "53");
                SaveScenario("Crypt Basement", 53, "54");
                SaveScenario("Palace of Ice", 54);
                SaveScenario("Foggy Thicket", 55, "56");
                SaveScenario("Bandits Wood", 56);
                SaveScenario("Investigation", 57, "58");
                SaveScenario("Bloody Shack", 58);
                SaveScenario("Forgotton Grove", 59, "60");
                SaveScenario("Alchemy Lab", 60);
                SaveScenario("Fading Lighthouse", 61, "62");
                SaveScenario("Pit of Souls", 62);
                SaveScenario("Magma Pit", 63);
                SaveScenario("Underwater Lagoon", 64, requiredGlobalAchievements: "9");
                SaveScenario("Sulfur Mine", 65);
                SaveScenario("Clockwork Cove", 66);
                SaveScenario("Arcane Library", 67);
                SaveScenario("Toxic Moor", 68);
                SaveScenario("Well of the Unfortunate", 69);
                SaveScenario("Chained Isle", 70);
                SaveScenario("Windswept Highlands", 71);
                SaveScenario("Oozing Grove", 72);
                SaveScenario("Rockslide Ridge", 73);
                SaveScenario("Merchant Ship", 74, requiredPartyAchievements: "12");
                SaveScenario("Overgrown Graveyard", 75, requiredPartyAchievements: "11");
                SaveScenario("Harrower Hive", 76);
                SaveScenario("Vault of Secrets", 77);
                SaveScenario("Sacrifice Pit", 78);
                SaveScenario("Lost Temple", 79, requiredPartyAchievements: "9");
                SaveScenario("Vigil Keep", 80);
                SaveScenario("Temple of the Eclipse", 81);
                SaveScenario("Burning Mountain", 82);
                SaveScenario("Shadows Within", 83, requiredPartyAchievements: "5");
                SaveScenario("Crystalline Cave", 84, requiredPartyAchievements: "26");
                SaveScenario("Sun Temple", 85);
                SaveScenario("Harried Village", 86, "87");
                SaveScenario("Corrupted Cove", 87, requiredPartyAchievements: "19");
                SaveScenario("Plane of Water", 88, requiredPartyAchievements: "27", requiredGlobalAchievements: "9");
                SaveScenario("Syndicate Hideout", 89, requiredPartyAchievements: "15");
                SaveScenario("Demonic Rift", 90);
                SaveScenario("Wild Melee", 91);
                SaveScenario("Back Alley Brawl", 92, requiredPartyAchievements: "7");
                SaveScenario("Sunken Vessel", 93, requiredPartyAchievements: "2");
                SaveScenario("Vermling Nest", 94, "95");
                SaveScenario("Payment Due", 95, requiredPartyAchievements: "23");
                Connection.Commit();
            }
            catch
            {
                Connection.Rollback();
                throw;
            }
        }

        private static void SaveScenario(string name, int scenarionumber,
                                         string unlockedScenarioIdsCommaSeparated = "",
                                         string requiredGlobalAchievements = "",
                                         string requiredPartyAchievements = "",
                                         string blockingGlobalAchievements = "",
                                         string blockingPartyAchievements = "")
        {
            var scenario = new DL_Scenario
            {
                Name = name,
                Scenarionumber = scenarionumber,
                UnlockedScenarios = unlockedScenarioIdsCommaSeparated,
                RequiredGlobalAchievements = requiredGlobalAchievements,
                RequiredPartyAchievements = requiredPartyAchievements,
                BlockingGlobalAchievements = blockingGlobalAchievements,
                BlockingPartyAchievements = blockingPartyAchievements
            };

            ScenarioRepository.InsertOrReplace(scenario);
        }

    }
}