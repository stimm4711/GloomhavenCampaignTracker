using GloomhavenCampaignTracker.Shared.Data.DatabaseAccess;
using GloomhavenCampaignTracker.Shared.Data.Entities;
using System;
using System.Collections.Generic;

namespace GloomhavenCampaignTracker.Shared.Data.Repositories
{
    public class CharacterRepository
    {
        private readonly CharacterDBAccess _db;
        protected static CharacterRepository Me;

        static CharacterRepository()
        {
            Me = new CharacterRepository();
        }
        public static bool IsInitilized()
        {
            return Me == null;
        }
        public static void Initialize()
        {
            Me = new CharacterRepository();
        }
        protected CharacterRepository()
        {
            _db = new CharacterDBAccess();
        }

        public static DL_Character Get(long id, bool recursive = true)
        {
            return Me._db.Get(id, recursive);
        }

        internal static List<DL_Item> GetItemsOfCharacter(int id)
        {
            return Me._db.GetItemsOfCharacter(id);
        }

        internal static DL_CampaignParty GetPartyOfCharacter(int id)
        {
            return Me._db.GetPartyOfCharacter(id);
        }

        internal static List<DL_Character> GetPartymembers(int id)
        {
            return Me._db.GetPartymembers(id);
        }

        internal static List<DL_Character> GetPartymembersFlat(int id)
        {
            return Me._db.GetPartymembersFlat(id);
        }

        internal static List<DL_Character> GetPartymembersUnretiredFlat(int id)
        {
            return Me._db.GetPartymembersUnretiredFlat(id);
        }

        internal static List<DL_Character> GetCharactersUnretiredFlat()
        {
            return Me._db.GetCharactersUnretiredFlat();
        }

        internal static List<DL_Character> GetCharactersFlat()
        {
            return Me._db.Get(recursive: false);
        }

        internal static List<DL_ClassPerk> GetClassPerks(int classid, int characterid)
        {
            return Me._db.GetClassPerks(classid, characterid);
        }

        internal static List<DL_PersonalQuest> GetPersonalQuestsFlat()
        {
            return Me._db.GetPersonalQuestsFlat();
        }

        internal static void Insert(IEnumerable<DL_Character> items)
        {
            Me._db.Insert(items);
        }

        public static List<DL_Character> Get(bool recursive = true)
        {
            return Me._db.Get(recursive);
        }

        public static void InsertOrReplace(DL_Character item)
        {
            Me._db.InsertOrReplace(item);
        }

        internal static void InsertOrReplace(IEnumerable<DL_Character> items)
        {
            Me._db.InsertOrReplace(items);
        }

        public static void Delete(DL_Character item)
        {
            Me._db.Delete(item);
        }
    }
}