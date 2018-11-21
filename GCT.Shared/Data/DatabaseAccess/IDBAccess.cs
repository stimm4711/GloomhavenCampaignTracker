using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.DatabaseAccess
{
    public interface IDBAccess<T> where T : class, IEntity
    {
        List<T> Get(bool recursive = true);

        T Get(long id, bool recursive = true);

        void Delete(T item);

        void InsertOrReplace(T item);
    }
}