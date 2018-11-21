using GloomhavenCampaignTracker.Shared.Business;
using NUnit.Framework;

namespace GloomhavenCampaignTracker.UnitTests.Business
{
    [TestFixture]
    public class EventDeckFixture
    {
        private EventDeck m_eventDeck;

        [SetUp]
        public void Setup()
        {
            m_eventDeck = new EventDeck();
        }

        [TearDown]
        public void Tear()
        {
            m_eventDeck = null;
        }

        [Test]
        public void AddCard()
        {
            m_eventDeck.AddCard(1);
            Assert.AreEqual(m_eventDeck.Count(), 1);

            m_eventDeck.AddCard(2);
            Assert.AreEqual(m_eventDeck.Count(), 2);

            m_eventDeck.AddCard(1);
            Assert.AreEqual(m_eventDeck.Count(), 2);
        }

        [Test]
        public void RemoveCard()
        {
            m_eventDeck.AddCard(1);
            m_eventDeck.AddCard(2);

            m_eventDeck.RemoveCard(2);
            Assert.AreEqual(m_eventDeck.Count(), 1);

            m_eventDeck.RemoveCard(2);
            Assert.AreEqual(m_eventDeck.Count(), 1);
        }

        [Test]
        public void DrawCard()
        {
            m_eventDeck.AddCard(1);
            m_eventDeck.AddCard(2);

            int cardId = m_eventDeck.DrawCard();
            m_eventDeck.RemoveCard(1);

            Assert.AreEqual(cardId, 1);
            Assert.AreEqual(1, m_eventDeck.Count());
        }

        [Test]
        public void Shuffle()
        {
            m_eventDeck.AddCard(1);
            m_eventDeck.AddCard(2);
            m_eventDeck.AddCard(3);
            m_eventDeck.AddCard(4);

            m_eventDeck.Shuffle();
        }

        [Test]
        public void ConvertToString()
        {
            m_eventDeck.AddCard(1);
            m_eventDeck.AddCard(2);
            m_eventDeck.AddCard(3);
            m_eventDeck.AddCard(4);

            string result = m_eventDeck.ToString();

            Assert.AreEqual("1,2,3,4", result );
        }

        [Test]
        public void ConvertFromString()
        {
            string cards = "1,2,3,4";

            m_eventDeck.FromString(cards);

            Assert.AreEqual(4, m_eventDeck.Count());
            Assert.AreEqual(1, m_eventDeck.DrawCard());
            m_eventDeck.RemoveCard(1);
            m_eventDeck.AddCard(1);
            Assert.AreEqual(2, m_eventDeck.DrawCard());
            m_eventDeck.RemoveCard(2);
            m_eventDeck.AddCard(2);
            Assert.AreEqual(3, m_eventDeck.DrawCard());
            m_eventDeck.RemoveCard(3);
            m_eventDeck.AddCard(3);
            Assert.AreEqual(4, m_eventDeck.DrawCard());
        }

        [Test]
        public void Initialize()
        {           
            m_eventDeck.InitializeDeck();
            Assert.AreEqual(31, m_eventDeck.Count());

            var items = m_eventDeck.GetItems();
            for(int i=1;i<=31;i++)
            {
                Assert.IsTrue(items.Contains(i));
            }                    
        }

    }
}