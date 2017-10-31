using Harmony;
using UnityEngine;
using Verse;

namespace StuffCount
{
    [HarmonyPatch(typeof(Dialog_InfoCard))]
    [HarmonyPatch("DoWindowContents")]
    static class Dialog_InfoCard_DoWindowContents_Patch
    {

        static void Postfix(Dialog_InfoCard __instance, Rect inRect)
        {
            ThingDef tDef = Traverse.Create(__instance).Property("Def").GetValue() as ThingDef;
            if (tDef == null) return;
            bool result=false;
            Utils.MyInfoCardButton(inRect.xMax - 48, inRect.yMin, tDef, ref result);
        }
    }
}
