using Harmony;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace StuffCount
{

    [HarmonyPatch(typeof(Listing_TreeThingFilter))]
    [HarmonyPatch("DoThingDef")]
    public static class Listing_TreeThingFilter_DoThingDef_Patch
    {
        static Color MenuSectionBGFillColor = (Color)AccessTools.Field(typeof(Widgets), "MenuSectionBGFillColor").GetValue(typeof(Widgets));

        [HarmonyPostfix]
        public static void Postfix(Listing_TreeThingFilter __instance, ThingDef tDef, int nestLevel)
        {
            

            var map = Find.VisibleMap;
            int stuffCount = Utils.Count(tDef, map);

            var readjustedY = __instance.CurHeight - __instance.lineHeight - __instance.verticalSpacing;
            var rowright= __instance.ColumnWidth - 28f;

            if (stuffCount > 0)
            {
                float right = rowright;

                List<ThingDef> _suppressSmallVolumeTags = (List<ThingDef>)Traverse.Create(__instance).Field("suppressSmallVolumeTags").GetValue();
                bool flag = (_suppressSmallVolumeTags == null || !_suppressSmallVolumeTags.Contains(tDef)) && tDef.IsStuff && tDef.smallVolume;
                string count = stuffCount.ToString() + (flag ? ("x" + 10.ToStringCached()) : "");
                var size = Text.CalcSize(count);
                float left = right - size.x;


                var rect = new Rect(left, readjustedY, right - left, __instance.lineHeight);

                GUI.color = MenuSectionBGFillColor;
                GUI.DrawTexture(rect, BaseContent.WhiteTex);
                //Widgets.DrawHighlightIfMouseover(rect);
                GUI.color = Color.grey;
                Text.Anchor = TextAnchor.MiddleRight;
                Widgets.Label(rect, count);
                Text.Anchor = TextAnchor.UpperLeft;
                GUI.color = Color.white;
            }

            Rect rectInfBtn = new Rect()
            {
                x = (float)nestLevel * __instance.nestIndentWidth + 18f,
                y = readjustedY,
                xMax = rowright,
                height = __instance.lineHeight
            };

            if(Widgets.ButtonInvisible(rectInfBtn, false))
            {
                if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Utils.ShowInfo(tDef);
                }
            }
        }
    }

}
