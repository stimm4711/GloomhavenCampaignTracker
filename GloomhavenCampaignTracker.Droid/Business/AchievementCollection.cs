using System.Collections.Generic;
using GloomhavenCampaignTracker.Shared.Data.Repositories;

namespace GloomhavenCampaignTracker.Business
{
    /// <summary>
    /// Enumeration of global achievements with internal numbers
    /// </summary>
    public enum GlobalAchievementsInternalNumbers
    {
        TheDrake_Slain = 101,
        TheDrake_Aided = 102,
        CityRule_Militaristic = 201,
        CityRule_Economic = 202,
        CityRule_Demonic = 203,
        Artifact_Recovered = 301,
        Artifact_Lost = 302,
        Artifact_Cleansed = 303,
        TheVoice_Silenced = 401,
        TheVoice_Freed = 402,
        TheMerchantFlees = 5,
        TheDeadInvade = 6,
        TheEdgeOfDarkness = 7,
        ThePowerOfEnhancement = 8,
        WaterBreathing = 9,
        TheDemonDethroned = 10,
        TheRiftClosed = 11,
        EndOfTheInvasion = 12,
        EndOfCurruption_Step1 = 131,
        EndOfCurruption_Step2 = 132,
        EndOfCurruption_Step3 = 133,
        EndOfGloom = 14,
        AncientTechnology_Step1 = 151,
        AncientTechnology_Step2 = 152,
        AncientTechnology_Step3 = 153,
        AncientTechnology_Step4 = 154,
        AncientTechnology_Step5 = 155,
        AnnihilationOfTheOrder = 16,
        ThroughThePortal = 17,
        KnowledgeIsPowerStep1 = 181,
        KnowledgeIsPowerStep2 = 182,
        PiecesOfAnArtifact = 19,
        APerilAvertedStep1 = 2001,
        APerilAvertedStep2 = 2002,
        MechanicalSplendor = 21,
        SeveredTies = 22 
    }

    /// <summary>
    /// enumeration of party achievements with internal numbers
    /// </summary>
    public enum PartyAchievementsInternalNumbers
    {
        ADemonsErrand = 1,
        AMaptoTreasure = 2,
        AcrosstheDivide = 3,
        AnInvitation = 4,
        BadBusiness = 5,
        DarkBounty = 6,
        DebtCollection = 7,
        FirstSteps = 8,
        FishsAid = 9,
        FollowingClues = 10,
        GraveJob = 11,
        HighSeaEscort = 12,
        JekserahsPlans = 13,
        RedthornsAid = 14,
        SinRa = 15,
        StonebreakersCenser = 16,
        TheDrakesCommand = 17,
        TheDrakesTreasure = 18,
        ThePoisonsSource = 19,
        TheScepterandtheVoice = 20,
        TheVoicesCommand = 21,
        TheVoicesTreasure = 22,
        ThroughtheNest = 23,
        ThroughtheRuins = 24,
        ThroughtheTrench = 25,
        Tremors = 26,
        WaterStaff = 27,
        SunBlessed = 28,
        HuntingTheHunter = 29,
        GuardDetail = 30,
        DimensionalEquilibrium = 31,
        AngelsOfDeath = 32,
        DiamarasAid = 33,
        HuntedPrey = 34,
        Accomplices = 35,
        Saboteurs = 36,
        Custodians = 37,
        AStrongbox = 38,
        Opportunists = 39,
        BeautyInFreedom = 40

    }

    public class AchievementCollection
    {
        private Dictionary<int, string> _dictPartyAchievementNames;

        public AchievementCollection()
        {
            LoadPartyAchievements();
        }

        private void LoadPartyAchievements()
        {
            var partyAchievements = PartyAchievementRepository.Get();
            _dictPartyAchievementNames = new Dictionary<int, string>();
            foreach (var pa in partyAchievements)
            {
                PartyAchievements.Add(new PartyAchievement(pa));

                if (!int.TryParse(pa.InternalNumber.ToString(), out int paNumber)) continue;
                _dictPartyAchievementNames.Add(paNumber, pa.Name);
            }
        }       

        public List<PartyAchievement> PartyAchievements { get; } = new List<PartyAchievement>();

        public string GlobalAchievementInternalNumberToName(int internalNumber)
        {
            switch (internalNumber)
            {
                case (int)GlobalAchievementsInternalNumbers.AncientTechnology_Step1:
                    return "Ancient Technology: 1/5";
                case (int)GlobalAchievementsInternalNumbers.AncientTechnology_Step2:
                    return "Ancient Technology: 2/5";
                case (int)GlobalAchievementsInternalNumbers.AncientTechnology_Step3:
                    return "Ancient Technology: 3/5";
                case (int)GlobalAchievementsInternalNumbers.AncientTechnology_Step4:
                    return "Ancient Technology: 4/5";
                case (int)GlobalAchievementsInternalNumbers.AncientTechnology_Step5:
                    return "Ancient Technology: 5/5";
                case (int)GlobalAchievementsInternalNumbers.AnnihilationOfTheOrder:
                    return "Annihilation of the Order";
                case (int)GlobalAchievementsInternalNumbers.Artifact_Cleansed:
                    return "Artifact: Cleansed";
                case (int)GlobalAchievementsInternalNumbers.Artifact_Lost:
                    return "Artifact: Lost";
                case (int)GlobalAchievementsInternalNumbers.Artifact_Recovered:
                    return "Artifact: Recovered";
                case (int)GlobalAchievementsInternalNumbers.CityRule_Demonic:
                    return "CityRule: Demonic";
                case (int)GlobalAchievementsInternalNumbers.CityRule_Economic:
                    return "CityRule: Economic";
                case (int)GlobalAchievementsInternalNumbers.CityRule_Militaristic:
                    return "CityRule: Militaristic";
                case (int)GlobalAchievementsInternalNumbers.EndOfCurruption_Step1:
                    return "End of Curruption: 1/3";
                case (int)GlobalAchievementsInternalNumbers.EndOfCurruption_Step2:
                    return "End of Curruption: 2/3";
                case (int)GlobalAchievementsInternalNumbers.EndOfCurruption_Step3:
                    return "End of Curruption: 3/3";
                case (int)GlobalAchievementsInternalNumbers.EndOfGloom:
                    return "End of Gloom";
                case (int)GlobalAchievementsInternalNumbers.EndOfTheInvasion:
                    return "End of the Invasion";
                case (int)GlobalAchievementsInternalNumbers.TheDeadInvade:
                    return "The Dead Invade";
                case (int)GlobalAchievementsInternalNumbers.TheDemonDethroned:
                    return "The Demon Dethroned";
                case (int)GlobalAchievementsInternalNumbers.TheDrake_Aided:
                    return "The Drake: Aided";
                case (int)GlobalAchievementsInternalNumbers.TheDrake_Slain:
                    return "The Drake: Slain";
                case (int)GlobalAchievementsInternalNumbers.TheEdgeOfDarkness:
                    return "The Edge of Darkness";
                case (int)GlobalAchievementsInternalNumbers.TheMerchantFlees:
                    return "The Merchant Flees";
                case (int)GlobalAchievementsInternalNumbers.ThePowerOfEnhancement:
                    return "The Power of Enhancement";
                case (int)GlobalAchievementsInternalNumbers.TheRiftClosed:
                    return "The Rift Closed";
                case (int)GlobalAchievementsInternalNumbers.TheVoice_Freed:
                    return "The Voice: Freed";
                case (int)GlobalAchievementsInternalNumbers.TheVoice_Silenced:
                    return "The Voice: Silenced";
                case (int)GlobalAchievementsInternalNumbers.WaterBreathing:
                    return "Water Breathing";
                case (int)GlobalAchievementsInternalNumbers.APerilAvertedStep1:
                    return "A Peril Averted: 1/2";
                case (int)GlobalAchievementsInternalNumbers.APerilAvertedStep2:
                    return "A Peril Averted: 2/2";
                case (int)GlobalAchievementsInternalNumbers.KnowledgeIsPowerStep1:
                    return "Knowledge is Power: 1/2";
                case (int)GlobalAchievementsInternalNumbers.KnowledgeIsPowerStep2:
                    return "Knowledge is Power: 2/2";
                case (int)GlobalAchievementsInternalNumbers.MechanicalSplendor:
                    return "Mechanical Splendor";
                case (int)GlobalAchievementsInternalNumbers.PiecesOfAnArtifact:
                    return "Pieces Of An Artifact";
                case (int)GlobalAchievementsInternalNumbers.SeveredTies:
                    return "Severed Ties";
                case (int)GlobalAchievementsInternalNumbers.ThroughThePortal:
                    return "Through the Portal";
                default:
                    return "Unknown";
            }
        }

        public string PartyAchievementInternalNumberToName(int internalNumber)
        {
            return _dictPartyAchievementNames.TryGetValue(internalNumber, out var value) ? value : "Unknown";
        }
    }
}