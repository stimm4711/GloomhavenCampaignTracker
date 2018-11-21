using System.Collections.Generic;
using System.Linq;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.RealUnitTests.Fakes
{
    public class DataServiceFake<T> : IDataService<T> where T :class, IEntity
    {
        private readonly List<T> _items = new List<T>();

        public void Delete(T item)
        {
            _items.Remove(item);
        }

        public T Get(long id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public List<T> Get()
        {
            return _items;
        }

        public void InsertOrReplace(T item)
        {
            var existingItem = _items.FirstOrDefault(y => y.Id == item.Id);                

            if (existingItem != null)
            {
                _items.Remove(existingItem);
            }
            else
            {
                var maxId = _items.Any() ? _items.Max(x => x.Id) : 0;
                item.Id = maxId + 1;
            }
               
            _items.Add(item);
        }
    }
}