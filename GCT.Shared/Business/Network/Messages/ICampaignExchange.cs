namespace Business.Network.Messages
{
    public interface ICampaignExchange
    {
        string Name { get; set; }

        int CityProsperity { get; set; }

        int DonatedGold { get; set; }

        string RoadEventDeckString { get; set; }

        string CityEventDeckString { get; set; }
    }

    public interface IPartyExchange
    {
        int Id { get; set; }

        string Name { get; set; }

        int Reputation { get; set; }

        int CurrentLocationNumber { get; set; }

        string Notes { get; set; }
    }

    public interface IPartyAchievementExchange
    {
        int ID_Party { get; set; }

        int ID_PartyAchievement { get; set; }
    }

    public interface ICampaignGlobalAchievementExchange
    {
        int ID_AchievementType { get; set; }

        int ID_Achievement { get; set; }
    }

    public interface ICampaignUnlocksExchange
    {
        bool EnvelopeAUnlocked { get; set; }

        bool EnvelopeBUnlocked { get; set; }

        bool TownRecordsBookUnlocked { get; set; }

        string UnlockedClassesIds { get; set; }

        string UnlockedItemDesignNumbers { get; set; }

        bool ReputationPlus10Unlocked { get; set; }

        bool ReputationPlus20Unlocked { get; set; }

        bool ReputationMinus10Unlocked { get; set; }

        bool ReputationMinus20Unlocked { get; set; }

        bool TheDrakePartyAchievementsUnlocked { get; set; }
    }

    public interface ICampaignEventHistoryLogItemExchange
    {
        int ReferenceNumber { get; set; }

        string Action { get; set; }

        string Outcome { get; set; }

        int Position { get; set; }

        int Decision { get; set; }

        int EventType { get; set; }
    }

    public interface ICampaignUnlockedItemExchange
    {
        int ID_Item { get; set; }

        int InStore { get; set; }
    }

    public interface ICampaignUnlockedScenarioExchange
    {
        int Id { get; set; }

        int ID_Scenario { get; set; }

        bool Completed { get; set; }

    }

    public interface ITreasureExchange
    {
        int Number { get; set; }

        bool Looted { get; set; }

        string Content { get; set; }

        int CampaignScenario_ID { get; set; }
    }

    public interface ICharacterExchange
    {
        int Id { get; set; }

        string Name { get; set; }

        int ClassId { get; set; }

        bool Retired { get; set; }

        int Level { get; set; }

        int Experience { get; set; }

        int Gold { get; set; }

        int LifegoalNumber { get; set; }

        int Checkmarks { get; set; }

        string Notes { get; set; }

        string Playername { get; set; }

        int ID_Party { get; set; }

        int ID_PersonalQuest { get; set; }

    }

    public interface ICharacterAbilityExchange
    {
        int Id { get; set; }

        string Name { get; set; }

        string AbilityName { get; set; }

        int ReferenceNumber { get; set; }

        int Level { get; set; }

        int ID_Character { get; set; }
    }

    public interface ICharacterItemExchange
    {
        int ID_Character { get; set; }

        int ID_Item { get; set; }
    }

    public interface ICharacterPerkExchange
    {
        int ID_Character { get; set; }

        int ID_ClassPerk { get; set; }
    }

    public interface IPerkExchange
    {
        int Checkboxnumber { get; set; }

        string Perkcomment { get; set; }

        int ID_Character { get; set; }
    }

    public interface IAbilityEnhancementExchange
    {
        int ID_Ability { get; set; }

        int ID_Enhancement { get; set; }

        int SlotNumber { get; set; }

        bool IsTop { get; set; }

    }

}
