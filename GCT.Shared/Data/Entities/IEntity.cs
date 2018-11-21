namespace GloomhavenCampaignTracker.Shared.Data.Entities
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface ISelectable
    {
        bool IsSelected { get; set; }
    }
}