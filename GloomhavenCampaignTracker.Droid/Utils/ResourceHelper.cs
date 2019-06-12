using System;

namespace GloomhavenCampaignTracker.Droid
{
    internal class ResourceHelper
    {
        public static int GetClassIconRessourceId(int classId)
        {
            // classid index between 0 and 18
            classId = Math.Max(Math.Min(classId, 18), 0);
            return m_classIcons[classId];
        }

        public static int GetClassIconWhiteSmallRessourceId(int classId)
        {
            // classid index between 0 and 18
            classId = Math.Max(Math.Min(classId, 18), 0);
            return m_classIconsWhiteSmall[classId];
        }

        public static int GetItemCategorieIconRessourceId(int categorieId)
        {
            // classid index between 0 and 16
            categorieId = Math.Max(Math.Min(categorieId, 5), 0);
            return m_itemcategorieItems[categorieId];
        }

        private static readonly int[] m_classIcons =
        {
            Resource.Drawable.ic_class1icon,
            Resource.Drawable.ic_class2icon,
            Resource.Drawable.ic_class3icon,
            Resource.Drawable.ic_class4icon,
            Resource.Drawable.ic_class5icon,
            Resource.Drawable.ic_class6icon,
            Resource.Drawable.ic_class7icon,
            Resource.Drawable.ic_class8icon,
            Resource.Drawable.ic_class9icon,
            Resource.Drawable.ic_class10icon,
            Resource.Drawable.ic_class11icon,
            Resource.Drawable.ic_class12icon,
            Resource.Drawable.ic_class13icon,
            Resource.Drawable.ic_class14icon,
            Resource.Drawable.ic_class15icon,
            Resource.Drawable.ic_class16icon,
            Resource.Drawable.ic_class17icon,
            Resource.Drawable.ic_class18icon_b,
            Resource.Drawable.ic_divinericon
        };

        private static readonly int[] m_classIconsWhiteSmall =
        {
            Resource.Drawable.ic_class1icon_white_48,
            Resource.Drawable.ic_class2icon_white_48,
            Resource.Drawable.ic_class3icon_white_48,
            Resource.Drawable.ic_class4icon_white_48,
            Resource.Drawable.ic_class5icon_white_48,
            Resource.Drawable.ic_class6icon_white_48,
            Resource.Drawable.ic_class7icon_white_48,
            Resource.Drawable.ic_class8icon_white_48,
            Resource.Drawable.ic_class9icon_white_48,
            Resource.Drawable.ic_class10icon_white_48,
            Resource.Drawable.ic_class11icon_white_48,
            Resource.Drawable.ic_class12icon_white_48,
            Resource.Drawable.ic_class13icon_white_48,
            Resource.Drawable.ic_class14icon_white_48,
            Resource.Drawable.ic_class15icon_white_48,
            Resource.Drawable.ic_class16icon_white_48,
            Resource.Drawable.ic_class17icon_white_48,
            Resource.Drawable.ic_class18icon_white,
            Resource.Drawable.ic_diviner_white_48

        };

        private static readonly int[] m_itemcategorieItems =
        {
            Resource.Drawable.ic_gloom_item_helmet,
            Resource.Drawable.ic_gloom_item_armor,
            Resource.Drawable.ic_gloom_item_onehand,
            Resource.Drawable.ic_gloom_item_twohand,
            Resource.Drawable.ic_gloom_item_boots,
            Resource.Drawable.ic_gloom_item_small,
        };
    }
}