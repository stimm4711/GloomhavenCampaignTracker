using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using System;
using Java.IO;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.Diagnostics;

namespace GloomhavenCampaignTracker.Business
{
    internal class BackupHandler
    {
        public static string backuppath = "ghcampaigntracker/backup/";

        internal static bool RestoreBackup(IDisposable sd, Java.IO.File dbfile)
        {
            if(RestoreDBBackup($"{sd}/{backuppath}{dbfile.Name}"))
            {
                GCTContext.Clear();
                GloomhavenDbHelper.ResetConnection();
                InitReposAfterRestoreBackup();

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool RestoreDBBackup(string dbfilepath)
        {
            try
            {
                var currentDBPath = GloomhavenDbHelper.DatabaseFilePath;
                var currentDB = new File(currentDBPath);
                var backupDB = new File(dbfilepath);

                if (!backupDB.Exists()) return false;

                var dst = new FileOutputStream(currentDB).Channel;
                var src = new FileInputStream(backupDB).Channel;
                dst.TransferFrom(src, 0, src.Size());
                src.Close();
                dst.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static void InitReposAfterRestoreBackup()
        {
            GloomhavenSettingsRepository.Initialize();
            AbilityRepository.Initialize();
            AchievementRepository.Initialize();
            AchievementTypeRepository.Initialize();
            CampaignEventHistoryLogItemRepository.Initialize();
            CampaignGlobalAchievementRepository.Initialize();
            CampaignPartyRepository.Initialize();
            CampaignRepository.Initialize();
            CampaignUnlockedItemRepository.Initialize();
            CampaignUnlockedScenarioRepository.Initialize();
            CampaignUnlocksRepository.Initialize();
            CharacterRepository.Initialize();
            ClassPerkRepository.Initialize();
            EnhancementRepository.Initialize();
            ItemRepository.Initialize();
            PartyAchievementRepository.Initialize();
            PersonalQuestRepository.Initialize();
            ScenarioRepository.Initialize();
            PersonalQuestCountersRepository.Initialize();
            CharacterPersonalQuestCountersRepository.Initialize();

            DataServiceCollection.Clear();
        }

        public static bool? CreateDBBackup(ref string buname)
        {
            try
            {
                var sd = Android.OS.Environment.ExternalStorageDirectory;

                if (!Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState) ||
                    Android.OS.Environment.MediaMountedReadOnly.Equals(
                        Android.OS.Environment.ExternalStorageState)) return null;

                var currentDBPath = GloomhavenDbHelper.DatabaseFilePath;
                var now = DateTime.Now.ToString("yyyyMMdd_H-mm-ss");
                var backupDBPath = $"{backuppath}{now}_{GloomhavenDbHelper.DatabaseName}";
                var currentDB = new File(currentDBPath);
                var backupDB = new File(sd, backupDBPath);
                var backupfolder = new File(sd, backuppath);

                if (!currentDB.Exists()) return null;

                if (!backupfolder.Exists())
                {
                    backupfolder.Mkdirs();
                }

                var src = new FileInputStream(currentDB).Channel;
                var dst = new FileOutputStream(backupDB).Channel;
                dst.TransferFrom(src, 0, src.Size());
                src.Close();
                dst.Close();


                if (backupDB.Exists())
                {
                    buname = backupDB.Name;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
