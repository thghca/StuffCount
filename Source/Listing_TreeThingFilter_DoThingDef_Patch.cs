using Harmony;
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
        public static void Postfix(Listing_TreeThingFilter __instance, ThingDef tDef)
        {
            

            var map = Find.VisibleMap;
            int stuffCount = StuffCount.Count(tDef, map);

            if (stuffCount == 0)
                return;
            
            var readjustedY = __instance.CurHeight - __instance.lineHeight - __instance.verticalSpacing;
            float right = __instance.ColumnWidth - 28f;

            List<ThingDef> _suppressSmallVolumeTags = (List<ThingDef>)Traverse.Create(__instance).Field("suppressSmallVolumeTags").GetValue();
            bool flag = (_suppressSmallVolumeTags == null || !_suppressSmallVolumeTags.Contains(tDef)) && tDef.IsStuff && tDef.smallVolume;
            string count = stuffCount.ToString() + (flag ? ("x" + 10.ToStringCached()) : "");
            var size = Text.CalcSize(count );
            float left = right - size.x;


            var rect = new Rect(left, readjustedY,right-left, __instance.lineHeight);
            
            GUI.color = MenuSectionBGFillColor;
            GUI.DrawTexture(rect, BaseContent.WhiteTex);
            Widgets.DrawHighlightIfMouseover(rect);
            GUI.color = Color.grey;
            Text.Anchor = TextAnchor.MiddleRight;
            Widgets.Label(rect, count);
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
        }
    }

}
