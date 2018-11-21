using GloomhavenCampaignTracker.Shared.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GloomhavenCampaignTracker.RealUnitTests.Shared.Business
{
    [TestClass]
    public class EventDeckFixture
    {
        private EventDeck m_eventDeck;

        [TestInitialize]
        public void Setup()
        {
            m_eventDeck = new EventDeck();
        }

        [TestCleanup]
        public void Tear()
        {
            m_eventDeck = null;
        }

        [TestMethod]
        public void AddCard()
        {
            m_eventDeck.AddCard(1);
            Assert.AreEqual(m_eventDeck.GetItems().Count, 1);

            m_eventDeck.AddCard(2);
            Assert.AreEqual(m_eventDeck.GetItems().Count, 2);

            m_eventDeck.AddCard(1);
            Assert.AreEqual(m_eventDeck.GetItems().Count, 2);
        }

        [TestMethod]
        public void RemoveCard()
        {
            m_eventDeck.AddCard(1);
            m_eventDeck.AddCard(2);

            m_eventDeck.RemoveCard(2);
            Assert.AreEqual(m_eventDeck.GetItems().Count, 1);

            m_eventDeck.RemoveCard(2);
            Assert.AreEqual(m_eventDeck.GetItems().Count, 1);
        }

        [TestMethod]
        public void DrawCard()
        {
            m_eventDeck.AddCard(1,false);
            m_eventDeck.AddCard(2, false);

            var cardId = m_eventDeck.DrawCard();
            m_eventDeck.RemoveCard(1);

            Assert.AreEqual(cardId, 1);
            Assert.AreEqual(1, m_eventDeck.GetItems().Count);
        }

        [TestMethod]
        public void Shuffle()
        {
            m_eventDeck.AddCard(1);
            m_eventDeck.AddCard(2);
            m_eventDeck.AddCard(3);
            m_eventDeck.AddCard(4);

            m_eventDeck.Shuffle();
        }

        [TestMethod]
        public void ConvertToString()
        {
            m_eventDeck.AddCard(1, false);
            m_eventDeck.AddCard(2, false);
            m_eventDeck.AddCard(3, false);
            m_eventDeck.AddCard(4, false);

            var result = m_eventDeck.ToString();

            Assert.AreEqual("1,2,3,4", result );
        }

        [TestMethod]
        public void ConvertFromString()
        {
            const string cards = "1,2,3,4";

            m_eventDeck.FromString(cards);

            Assert.AreEqual(4, m_eventDeck.GetItems().Count);
            Assert.AreEqual(1, m_eventDeck.DrawCard());
            m_eventDeck.RemoveCard(1);
            m_eventDeck.AddCard(1, false);
            Assert.AreEqual(2, m_eventDeck.DrawCard());
            m_eventDeck.RemoveCard(2);
            m_eventDeck.AddCard(2, false);
            Assert.AreEqual(3, m_eventDeck.DrawCard());
            m_eventDeck.RemoveCard(3);
            m_eventDeck.AddCard(3, false);
            Assert.AreEqual(4, m_eventDeck.DrawCard());
        }

        [TestMethod]
        public void Initialize()
        {           
            m_eventDeck.InitializeDeck();
            Assert.AreEqual(30, m_eventDeck.GetItems().Count);

            var items = m_eventDeck.GetItems();
            for(var i=1;i<31;i++)
            {
                Assert.IsTrue(items.Contains(i));
            }                    
        }

    }
}