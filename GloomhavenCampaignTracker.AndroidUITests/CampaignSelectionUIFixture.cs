using GloomhavenCampaignTracker.Droid;
using NUnit.Framework;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Android;

namespace GloomhavenCampaignTracker.AndroidUITests
{
    [TestFixture]
    public class CampaignSelectionUIFixture
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            // TODO: If the Android app being tested is included in the solution then open
            // the Unit Tests window, right click Test Apps, select Add App Project
            // and select the app projects that should be tested.
            app = ConfigureApp.Android
                .ApkFile("../../../GloomhavenCampaignTracker.Droid/bin/Release/de.timmcode.ghcampaigntracker.droid.apk")
                .EnableLocalScreenshots()
                .StartApp();
        }

        [Test]
        public void CreateNewCampaign()
        {
            const string campaignname = "Journey";
            const string partyname = "Legendary Unknows";

            app.Screenshot("Start up to campaign selection.");

            Tap(CampaignSelectionUiElements.CreateNewCampaignFloatingActionButton);
            app.Screenshot("Create new campaign dialog is open.");

            EnterText(CampaignSelectionUiElements.CreateCampaignDialogCampaignNameTextView, campaignname);
            EnterText(CampaignSelectionUiElements.CreateCampaignDialogPartyNameTextView, partyname);
            app.Screenshot("campaign and partyname entered");

            Tap(CampaignSelectionUiElements.StartNewCampaignDialogButton);

            app.Screenshot("campaign created and campaign view visible");

            Assert.IsTrue(app.Query(c => c.Class("TextView").Text("Journey")).Any());
            Assert.IsTrue(app.Query(c => c.Id("tabText").Text("World")).Any());
            Assert.IsTrue(app.Query(c => c.Id("tabText").Text("City")).Any());
            Assert.IsTrue(app.Query(c => c.Id("tabText").Text("Party")).Any());
            Assert.IsTrue(app.Query(c => c.Id("tabText").Text("Unlocks")).Any());

            app.Screenshot("campaign created and campaign view visible");

            Tap(CampaignViewPagerUIElements.PartyTab);

            Assert.IsTrue(app.Query(c => c.Id("partynameTextView").Text("Legendary Unknows")).Any());
            app.Screenshot("Partyname");            
        }

        [Test]
        public void CreateCampaignTwice()
        {
            const string campaignname = "Journey";
            const string partyname = "Legendary Unknows";

            app.Screenshot("Start up to campaign selection.");

            Tap(CampaignSelectionUiElements.CreateNewCampaignFloatingActionButton);
            app.Screenshot("Create new campaign dialog is open.");

            EnterText(CampaignSelectionUiElements.CreateCampaignDialogCampaignNameTextView, campaignname);
            EnterText(CampaignSelectionUiElements.CreateCampaignDialogPartyNameTextView, partyname);
            app.Screenshot("campaign and partyname entered");

            Tap(CampaignSelectionUiElements.StartNewCampaignDialogButton);

            app.Screenshot("campaign created and campaign view visible");

            Assert.IsTrue(app.Query(c => c.Class("TextView").Text("Journey")).Any());
            Assert.IsTrue(app.Query(c => c.Id("tabText").Text("World")).Any());
            Assert.IsTrue(app.Query(c => c.Id("tabText").Text("City")).Any());
            Assert.IsTrue(app.Query(c => c.Id("tabText").Text("Party")).Any());
            Assert.IsTrue(app.Query(c => c.Id("tabText").Text("Unlocks")).Any());

            app.Screenshot("campaign created and campaign view visible");

            Tap(CampaignViewPagerUIElements.PartyTab);

            Assert.IsTrue(app.Query(c => c.Id("partynameTextView").Text("Legendary Unknows")).Any());
            app.Screenshot("Partyname");

            Tap(CampaignViewPagerUIElements.CampaignSelectionActionBarMenuButton);
            Tap(CampaignSelectionUiElements.CreateNewCampaignFloatingActionButton);

            EnterText(CampaignSelectionUiElements.CreateCampaignDialogCampaignNameTextView, campaignname);
            EnterText(CampaignSelectionUiElements.CreateCampaignDialogPartyNameTextView, partyname);

            Tap(CampaignSelectionUiElements.StartNewCampaignDialogButton);
        }

        private void Tap(string uielement)
        {
            app.Tap(c => c.Marked(uielement));
        }

        private void EnterText(string uielement, string text)
        {
            app.EnterText(c => c.Marked(uielement), text);
        }       

        private class CampaignSelectionUiElements
        {
            public static string StartNewCampaignDialogButton = "Start new Campaign";

            public static string CreateNewCampaignFloatingActionButton = "fab";

            public static string CreateCampaignDialogCampaignNameTextView = "campaigname";

            public static string CreateCampaignDialogPartyNameTextView = "partyname";
        }

        private class CampaignViewPagerUIElements
        {
            public static string PartyTab = "Party";

            public static string CampaignSelectionActionBarMenuButton = "campaignSelection";
        }
    }
}

