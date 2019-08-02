using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GloomhavenCampaignTracker.Droid.Data.ConversionClasses
{
    public class JSONConverter
    {     
        public static List<T> LoadJson<T>(string jsonfile)
        {
            List<T> items = null;
            using (StreamReader r = new StreamReader(jsonfile))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<T>>(json);               
            }

            if (items != null)
                return items;
            else
                return new List<T>();
        }

        internal static T LoadJson<T>(Stream asset)
        {
            T items;
            using (StreamReader r = new StreamReader(asset))
            {
                string json = r.ReadToEnd();
                items = JsonConvert.DeserializeObject<T>(json);
            }

            if (items != null)
                return items;
            else
                return default(T);
        }
    }

    public class Ability
    {
        public string abilityName { get; set; }
        public int referenceNumber { get; set; }
        public int level { get; set; }
        public object image_url { get; set; }
        public int iD_Class { get; set; }
    }

    public class Classability
    {
        public string classname { get; set; }
        public int classid { get; set; }
        public List<Ability> Abilities { get; set; }
    }

    public class ClassAbilites
    {
        public List<Classability> classabilities { get; set; }
    }
}