using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using System;
using Java.IO;
using GloomhavenCampaignTracker.Shared.Data.Repositories;
using System.IO;
using System.Diagnostics;
using Java.Nio.Channels;

namespace GloomhavenCampaignTracker.Business
{
    internal class BackupHandler
    {
        internal static bool RestoreBackup(string backuppath)
        {
            if(RestoreDBBackup(backuppath))
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
                var currentDB = new Java.IO.File(currentDBPath);
                var backupDB = new Java.IO.File(dbfilepath);

                if (!backupDB.Exists()) return false;
                TransferFileStreams(backupDB , currentDB);

                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static void TransferFileStreams(Java.IO.File srcFile, Java.IO.File destFile)
        {
            var src = new FileInputStream(srcFile).Channel;
            Transfer(destFile, src);
        }

        internal static void TransferFileStreams(FileDescriptor sourceFileDescriptor, Java.IO.File destFile)
        {
            var src = new FileInputStream(sourceFileDescriptor).Channel;
            Transfer(destFile, src);
        }

        private static void Transfer(Java.IO.File destFile, FileChannel src)
        {
            var dst = new FileOutputStream(destFile).Channel;

            dst.TransferFrom(src, 0, src.Size());
            src.Close();
            dst.Close();
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

        public static bool? CreateDBBackup(ref string buname, string backuppath)
        {
            try
            {
                var sd = Android.OS.Environment.ExternalStorageDirectory;

                if (!Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState) ||
                    Android.OS.Environment.MediaMountedReadOnly.Equals(
                        Android.OS.Environment.ExternalStorageState)) return null;

                var currentDBPath = GloomhavenDbHelper.DatabaseFilePath;
                var now = DateTime.Now.ToString("yyyyMMdd_H-mm-ss");
                var backupDBPath = Path.Combine(backuppath, $"{now}_GHC_bak.db3");
                var currentDB = new Java.IO.File(currentDBPath);
                var backupDB = new Java.IO.File(backupDBPath);
                var backupfolder = new Java.IO.File(backuppath);

                if (!currentDB.Exists()) return null;

                if (!backupfolder.Exists())
                {
                    backupfolder.Mkdirs();
                }

                TransferFileStreams(currentDB, backupDB);

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
