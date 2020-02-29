using Android.App;
using Android.Content;
using Android.Preferences;

namespace GloomhavenCampaignTracker.Droid
{
    public class Settings
    {
        private const string _showItemnames = "showItemnames";
        private const string _showPQNames = "showPQDetails";
        private const string _showScenarioNames = "showScenarioNames";
        private const string _activateFC = "activateFC";
        private const string _showTreasures = "showTreasures";
        private const string _showReleaseNotes = "showReleaenotes";
        private const string _backuppath = "Backuppath";
        private const string _lastloadedParty = "lastloadedparty";
        private const string _filterRetired = "FilterRetired";
        private const string _lastloadedcampaign = "lastloadedcampaign";
        private const string _username = "username";
        private const string _downloadImages = "downloadImages";

        private bool isShowItems;
        private bool isShowPq;
        private bool isFCActivated;
        private bool isShowReleasenotes;
        private bool isShowScenarios;
        private bool isShowTreasure;
        private string backuppath;
        private int lastLoadedParty;
        private bool isFilterRetired;
        private int lastloadedcampaign;
        private string username;
        private bool isDownloadImages;

        public Settings()
        {
            ReadSettings();
        }

        public bool IsShowItems
        {
            get => isShowItems;
            set
            {
                isShowItems = value;
                SetSettingBoolean(_showItemnames, value);
            }
        }

        public bool IsShowPq
        {
            get => isShowPq;
            set
            {
                isShowPq = value;
                SetSettingBoolean(_showPQNames, value);
            }
        }

        public bool IsShowOldAbilitySheet
        {
            get => false;
        }

        public bool IsFCActivated
        {
            get => isFCActivated;
            set
            {
                isFCActivated = value;
                SetSettingBoolean(_activateFC, value);
            }
        }

        public bool IsShowReleasenotes
        {
            get => isShowReleasenotes;
            set
            {
                isShowReleasenotes = value;
                SetSettingBoolean(_showReleaseNotes, value);
            }
        }

        public bool IsShowScenarios
        {
            get => isShowScenarios;
            set
            {
                isShowScenarios = value;
                SetSettingBoolean(_showScenarioNames, value);
            }
        }

        public bool IsShowTreasure
        {
            get => isShowTreasure;
            set
            {
                isShowTreasure = value;
                SetSettingBoolean(_showTreasures, value);
            }
        }

        public string Backuppath
        {
            get => backuppath;
            set
            {
                backuppath = value;
                SetSettingString(_backuppath, value);
            }
        }

        public int LastLoadedParty
        {
            get => lastLoadedParty;
            set
            {
                lastLoadedParty = value;
                SetSettingInt(_lastloadedParty, value);
            }
        }

        public bool IsFilterRetired
        {
            get => isFilterRetired;
            set
            {
                isFilterRetired = value;
                SetSettingBoolean(_filterRetired, value);
            }
        }

        public int LastLoadedCampaign
        {
            get => lastloadedcampaign;
            set
            {
                lastloadedcampaign = value;
                SetSettingInt(_lastloadedcampaign, value);
            }
        }

        public string Username
        {
            get => username;
            set
            {
                username = value;
                SetSettingString(_username, value);
            }
        }

        public bool IsDownloadImages
        {
            get => isDownloadImages;
            set
            {
                isDownloadImages = value;
                SetSettingBoolean(_downloadImages, value);
            }
        }
 

        public void ReadSettings()
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            IsShowItems = prefs.GetBoolean(_showItemnames, true);
            IsShowPq = prefs.GetBoolean(_showPQNames, true);
            IsFCActivated = prefs.GetBoolean(_activateFC, false);
            IsShowReleasenotes = prefs.GetBoolean(_showReleaseNotes, true);
            IsShowScenarios = prefs.GetBoolean(_showScenarioNames, false);
            IsShowTreasure = prefs.GetBoolean(_showTreasures, true);
            Backuppath = prefs.GetString(_backuppath, $"{Android.OS.Environment.ExternalStorageDirectory}/ghcampaigntracker/backup/");
            LastLoadedParty = prefs.GetInt(_lastloadedParty, -1);
            IsFilterRetired = prefs.GetBoolean(_filterRetired, true);
            LastLoadedCampaign = prefs.GetInt(_lastloadedcampaign, -1);
            Username = prefs.GetString(_username, "");
            IsDownloadImages = prefs.GetBoolean(_downloadImages, false);
        }

        public void SetSettingBoolean(string settingname, bool value)
        {
            var editor = GetEditor();
            editor.PutBoolean(settingname, value);
            editor.Apply();
        }

        public void SetSettingString(string settingname, string value)
        {
            var editor = GetEditor();
            editor.PutString(settingname, value);
            editor.Apply();
        }

        public void SetSettingInt(string settingname, int value)
        {
            var editor = GetEditor();
            editor.PutInt(settingname, value);
            editor.Apply();
        }

        private ISharedPreferencesEditor GetEditor()
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            var editor = prefs.Edit();
            return editor;
        }

    }
}