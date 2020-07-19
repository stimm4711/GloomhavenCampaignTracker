using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Java.Lang;

namespace GloomhavenCampaignTracker.Droid
{
    public static class SpannableTools
    {
        private static readonly Dictionary<string, int> Icons;

        static SpannableTools()
        {
            Icons = new Dictionary<string, int>
            {
                {"[RM]", Resource.Drawable.RollingModifier},
                {"[PU]", Resource.Drawable.ic_effect_push},
                {"[PUL]", Resource.Drawable.ic_effect_pull},
                {"[PI]", Resource.Drawable.ic_effect_pierce},
                {"[ST]", Resource.Drawable.ic_status_stun},
                {"[D]", Resource.Drawable.ic_status_disarm},
                {"[I]", Resource.Drawable.ic_status_immobilized},
                {"[M]", Resource.Drawable.ic_status_muddle},
                {"[W]", Resource.Drawable.ic_status_wound},
                {"[T]", Resource.Drawable.ic_effect_addTarget},
                {"[SH]", Resource.Drawable.ic_effect_shield},
                {"[H]", Resource.Drawable.ic_effect_heal},
                {"[PO]", Resource.Drawable.ic_status_poisen },
                {"[C]", Resource.Drawable.ic_status_curse },
                {"[B]", Resource.Drawable.ic_status_bless },
                {"[IN]", Resource.Drawable.ic_status_invisible },
                {"[RE]", Resource.Drawable.ic_regenerate_self },
                {"[RE2]", Resource.Drawable.ic_regenerate },

                {"[-1]", Resource.Drawable.ic_effect_minus_one},
                {"[-2]", Resource.Drawable.ic_effect_minus_two},
                {"[+0]", Resource.Drawable.ic_effect_plus_zero},
                {"[+1]", Resource.Drawable.ic_effect_plus_one},
                {"[+2]", Resource.Drawable.ic_effect_plus_two},
                {"[+3]", Resource.Drawable.ic_effect_plus_three},
                {"[+4]", Resource.Drawable.ic_effect_plus_four},

                {"[ANY]",Resource.Drawable.ic_element_any  },
                {"[EARTH]",Resource.Drawable.ic_element_earth  },
                {"[WIND]",Resource.Drawable.ic_element_wind  },
                {"[FIRE]",Resource.Drawable.ic_element_fire  },
                {"[LIGHT]",Resource.Drawable.ic_element_light  },
                {"[DARK]",Resource.Drawable.ic_element_dark  },
                {"[FROST]",Resource.Drawable.ic_element_frost  },

                {"[MOVE]",Resource.Drawable.ic_ability_move  },
                {"[ATTACK]",Resource.Drawable.ic_ability_attack  },
                {"[RANGE]",Resource.Drawable.ic_ability_range  },
                {"[RET]",Resource.Drawable.ic_ability_retaliate  },

                {"[e+1]",Resource.Drawable.enhancement_plus_one  },

                { "[STR]",Resource.Drawable.ic_status_strengthen  },
                { "[J]",Resource.Drawable.ic_ability_jump  },
                { "[HEX]",Resource.Drawable.ic_ability_targethex  },

                { "[C1]", Resource.Drawable.ic_class1icon_white_48 },
                { "[C2]", Resource.Drawable.ic_class2icon_white_48 },
                { "[C3]", Resource.Drawable.ic_class3icon_white_48 },
                { "[C4]", Resource.Drawable.ic_class4icon_white_48 },
                { "[C5]", Resource.Drawable.ic_class5icon_white_48 },
                { "[C6]", Resource.Drawable.ic_class6icon_white_48 },
                { "[C7]", Resource.Drawable.ic_class7icon_white_48 },
                { "[C8]", Resource.Drawable.ic_class8icon_white_48 },
                { "[C9]", Resource.Drawable.ic_class9icon_white_48 },
                { "[C10]", Resource.Drawable.ic_class10icon_white_48 },
                { "[C11]", Resource.Drawable.ic_class11icon_white_48 },
                { "[C12]", Resource.Drawable.ic_class12icon_white_48 },
                { "[C13]", Resource.Drawable.ic_class13icon_white_48 },
                { "[C14]", Resource.Drawable.ic_class14icon_white_48 },
                { "[C15]", Resource.Drawable.ic_class15icon_white_48 },
                { "[C16]", Resource.Drawable.ic_class16icon_white_48 },
                { "[C17]", Resource.Drawable.ic_class17icon_white_48 },
                { "[C18]", Resource.Drawable.ic_class18icon_white },
                { "[C19]", Resource.Drawable.ic_diviner_white_48 },
                { "[C20]", Resource.Drawable.ic_class20icon_white_48 },
                { "[C21]", Resource.Drawable.ic_class21icon_white_48 },
                { "[C22]", Resource.Drawable.ic_class22icon_white_48 },
                { "[C23]", Resource.Drawable.ic_class23icon_white_48 },


                { "[PROSP]", Resource.Drawable.prosperity }
            };
        }

        public static bool AddIcons(Context context, ISpannable spannable, int lineheight)
        {            
            var hasChanges = false;
            if (Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop) return false;

            foreach (var entry in Icons)
            {
                var iconText = entry.Key;
                var icon = entry.Value;
                var indices = spannable.ToString().IndexesOf(iconText);
                foreach (var index in indices)
                {
                    var set = true;
                    foreach (var span in spannable.GetSpans(index, index + iconText.Length, Class.FromType(typeof(ImageSpan))))
                    {
                        if (spannable.GetSpanStart(span) >= index && spannable.GetSpanEnd(span) <= index + iconText.Length)
                            spannable.RemoveSpan(span);
                        else
                        {
                            set = false;
                            break;
                        }
                    }

                    if (!set) continue;

                    hasChanges = true;
                    var drawable = ContextCompat.GetDrawable(context, icon);
                    drawable.Bounds.Set(0, 0, lineheight, lineheight);

                    var imgSpan = new ImageSpan(drawable);
                    spannable.SetSpan(imgSpan, index, index + iconText.Length, SpanTypes.ExclusiveExclusive);
                }
            }
            return hasChanges;
        }

        public static void AddIcons(string textSmiley, int smileyResource)
        {
            Icons.Add(textSmiley, smileyResource);
        }
    }

    //Taken from http://stackoverflow.com/a/767788/368379
    public static class StringExtensions
    {
        public static IEnumerable<int> IndexesOf(this string haystack, string needle)
        {
            var lastIndex = 0;
            while (true)
            {
                if (haystack == null) continue;

                var index = haystack.IndexOf(needle, lastIndex, StringComparison.Ordinal);
                if (index == -1)
                {
                    yield break;
                }
                yield return index;
                lastIndex = index + needle.Length;
            }
        }
    }
}