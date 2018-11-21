using GloomhavenCampaignTracker.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GloomhavenCampaignTracker.RealUnitTests.Shared.Business
{
    [TestClass]
    public class HelperFixture
    {
        [TestMethod]
        public void GetShopPriceModifier_DictNotInitialized()
        {
            var modi = Helper.GetShopPriceModifier(0);

            Assert.AreEqual(0, modi);

            modi = Helper.GetShopPriceModifier(2);

            Assert.AreEqual(0, modi);

            modi = Helper.GetShopPriceModifier(3);

            Assert.AreEqual(-1, modi);

            modi = Helper.GetShopPriceModifier(6);

            Assert.AreEqual(-1, modi);

            modi = Helper.GetShopPriceModifier(7);

            Assert.AreEqual(-2, modi);

            modi = Helper.GetShopPriceModifier(10);

            Assert.AreEqual(-2, modi);

            modi = Helper.GetShopPriceModifier(11);

            Assert.AreEqual(-3, modi);

            modi = Helper.GetShopPriceModifier(14);

            Assert.AreEqual(-3, modi);

            modi = Helper.GetShopPriceModifier(15);

            Assert.AreEqual(-4, modi);

            modi = Helper.GetShopPriceModifier(18);

            Assert.AreEqual(-4, modi);

            modi = Helper.GetShopPriceModifier(19);

            Assert.AreEqual(-5, modi);

            modi = Helper.GetShopPriceModifier(20);

            Assert.AreEqual(-5, modi);

            modi = Helper.GetShopPriceModifier(-10);

            Assert.AreEqual(2, modi);

            modi = Helper.GetShopPriceModifier(-15);

            Assert.AreEqual(4, modi);
        }
    }
}