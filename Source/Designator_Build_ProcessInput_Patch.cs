using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace StuffCount
{

    [HarmonyPatch(typeof(Designator_Build))]
    [HarmonyPatch("ProcessInput")]
    public static class Designator_Build_ProcessInput_Patch
    {

        public static void Postfix(Designator_Build __instance, Event ev)
        {
            var _Map = ((Map)Traverse.Create(__instance).Property("Map").GetValue());
            var _entDef = (BuildableDef)Traverse.Create(__instance).Field("entDef").GetValue();

            ThingDef thingDef = _entDef as ThingDef;

            if (thingDef == null || !thingDef.MadeFromStuff)
            {
                return;
            }
            FloatMenu floatMenu = (FloatMenu)Find.WindowStack.Windows.SingleOrDefault(x => x.GetType() == typeof(FloatMenu)&&!x.GetType().IsSubclassOf(typeof(FloatMenu)));
            if (floatMenu == null) return;
            List<FloatMenuOption> optionsList = (List<FloatMenuOption>)AccessTools.Field(typeof(FloatMenu), "options").GetValue(floatMenu);
            foreach (ThingDef thingDef2 in _Map.resourceCounter.AllCountedAmounts.Keys)
            {
                if (thingDef2.IsStuff && thingDef2.stuffProps.CanMake(thingDef) && (DebugSettings.godMode || _Map.listerThings.ThingsOfDef(thingDef2).Count > 0))
                {
                    ThingDef localStuffDef = thingDef2;
                    string labelCap = localStuffDef.LabelCap;
                   
                    FloatMenuOption option = optionsList.SingleOrDefault(x => x.Label == labelCap);

                    if (option != null)
                    {

                        int stuffCount = Utils.Count(localStuffDef, _Map);
                        string labelCapNew = localStuffDef.LabelCap + " (" + stuffCount + ")";

                        option.Label = labelCapNew;
                    }
                }
            }
            Traverse.Create(floatMenu).Method("SetInitialSizeAndPosition").GetValue();
        }


    }

}
