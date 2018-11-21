using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Entities;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public interface IDataService<T> where T : class, IEntity
    {
        T Get(long id);

        List<T> Get();

        void InsertOrReplace(T item);

        void Delete(T item);
    }
}