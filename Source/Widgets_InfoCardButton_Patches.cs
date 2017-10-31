using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace StuffCount
{
    [HarmonyPatch(typeof(Widgets))]
    [HarmonyPatch("InfoCardButton")]
    [HarmonyPatch(new Type[] { typeof(float), typeof(float), typeof(Def) })]
    static class Widgets_InfoCardButton_Patch
    {
        static readonly MethodInfo infoCardButtonWorker = AccessTools.Method(typeof(Widgets), "InfoCardButtonWorker");
        static bool Prefix(float x, float y, Def def, ref bool __result)
        {
            ThingDef tDef = def as ThingDef;
            if (tDef == null) return true;
            return Utils.MyInfoCardButton(x, y, tDef, ref __result);
        }
    }

    [HarmonyPatch(typeof(Widgets))]
    [HarmonyPatch("InfoCardButton")]
    [HarmonyPatch(new Type[] { typeof(float), typeof(float), typeof(ThingDef), typeof(ThingDef) })]
    static class Widgets_InfoCardButton2_Patch
    {
        static readonly MethodInfo infoCardButtonWorker = AccessTools.Method(typeof(Widgets), "InfoCardButtonWorker");
        static bool Prefix(float x, float y, ThingDef thingDef, ThingDef stuffDef, ref bool __result)
        {
            return Utils.MyInfoCardButton(x, y, thingDef, ref __result);
        }
    }

    [HarmonyPatch(typeof(Widgets))]
    [HarmonyPatch("InfoCardButton")]
    [HarmonyPatch(new Type[] { typeof(float), typeof(float), typeof(Thing) })]
    static class Widgets_InfoCardButton3_Patch
    {
        
        static bool Prefix(float x, float y, Thing thing, ref bool __result)
        {
            return Utils.MyInfoCardButton(x, y, thing.def, ref __result);
        }
    }
}
