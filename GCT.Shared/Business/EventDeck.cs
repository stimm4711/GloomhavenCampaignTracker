using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GloomhavenCampaignTracker.Shared.Business
{
    public class EventDeck
    {
        private readonly List<int> _eventDeckItems = new List<int>();

        public void InitializeDeck()
        {
            _eventDeckItems.Clear();
            for (var i = 1; i<31; i++)
            {
                _eventDeckItems.Add(i);
            }

            Shuffle();
        }

        public void Shuffle()
        {
            var provider = new RNGCryptoServiceProvider();
            var n = _eventDeckItems.Count;
            while (n > 1)
            {
                var box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                var k = (box[0] % n);
                n--;
                var value = _eventDeckItems[k];
                _eventDeckItems[k] = _eventDeckItems[n];
                _eventDeckItems[n] = value;
            }
        }

        public void AddCard(int cardId, bool doShuffle = true)
        {
            if (!_eventDeckItems.Contains(cardId))
            {
                _eventDeckItems.Add(cardId);
            }            
            if (doShuffle) Shuffle();
        }

        public void PutBack(int cardId)
        {
            if (!_eventDeckItems.Contains(cardId))
            {
                _eventDeckItems.Add(cardId);
            }
        }

        public void RemoveCard(int cardId)
        {
            _eventDeckItems.Remove(cardId);
        }

        public int DrawCard()
        {
            if (_eventDeckItems.Any())
            {
                var cardId = _eventDeckItems.First();
                return cardId;
            }
            else
            {
                return -1;
            }   
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var i in _eventDeckItems)
            {
                sb.Append(i.ToString());
                sb.Append(",");
            }

            if(sb.Length > 0)
            {
                sb.Remove(sb.Length-1, 1);
            }
            
            return sb.ToString();
        }

        public void FromString(string eventDeckString)
        {
            _eventDeckItems.Clear();

            var cardIds = eventDeckString.Split(',');            
           
            foreach (var i in cardIds)
            {
                if(int.TryParse(i, out int cardId)) _eventDeckItems.Add(cardId);
            }
        }

        public List<int> GetItems()
        {
            return _eventDeckItems;
        }
    }

}

