using GloomhavenCampaignTracker.Shared;
using NUnit.Framework;

namespace GloomhavenCampaignTracker.UnitTests.Business
{
    public class HelperFixture
    {
        [Test]
        public void GetShopPriceModifier_DictNotInitialized()
        {
            var modi = Helper.GetShopPriceModifier(0);

            Assert.That(modi.Equals(0));

            modi = Helper.GetShopPriceModifier(2);

            Assert.That(modi.Equals(0));

            modi = Helper.GetShopPriceModifier(3);

            Assert.That(modi.Equals(-1));

            modi = Helper.GetShopPriceModifier(6);

            Assert.That(modi.Equals(-1));

            modi = Helper.GetShopPriceModifier(7);

            Assert.That(modi.Equals(-2));

            modi = Helper.GetShopPriceModifier(10);

            Assert.That(modi.Equals(-2));

            modi = Helper.GetShopPriceModifier(11);

            Assert.That(modi.Equals(-3));

            modi = Helper.GetShopPriceModifier(14);

            Assert.That(modi.Equals(-3));

            modi = Helper.GetShopPriceModifier(15);

            Assert.That(modi.Equals(-4));

            modi = Helper.GetShopPriceModifier(18);

            Assert.That(modi.Equals(-4));

            modi = Helper.GetShopPriceModifier(19);

            Assert.That(modi.Equals(-5));

            modi = Helper.GetShopPriceModifier(20);

            Assert.That(modi.Equals(-5));

            modi = Helper.GetShopPriceModifier(-10);

            Assert.That(modi.Equals(2));

            modi = Helper.GetShopPriceModifier(-15);

            Assert.That(modi.Equals(4));
        }
    }
}