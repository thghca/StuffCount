using Harmony;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace StuffCount
{

    [HarmonyPatch(typeof(FloatMenu))]
    [HarmonyPatch("get_ColumnWidth")]
    public static class FloatMenu_get_ColumnWidth_Patch
    {
        public static void Postfix(FloatMenu __instance, ref float __result)
        {
            var _options = (List<FloatMenuOption>)Traverse.Create(__instance).Field("options").GetValue();

            float MinimumColumnWidth = (float)Traverse.Create(typeof(FloatMenu)).Field("MinimumColumnWidth").GetValue();
            float MaximumColumnWidth = 600f;

            for (int i = 0; i < _options.Count; i++)
            {
                float requiredWidth = _options[i].RequiredWidth;
                if (requiredWidth >= MaximumColumnWidth)
                {
                    __result = MaximumColumnWidth;
                    return;
                }
                if (requiredWidth > MinimumColumnWidth)
                {
                    MinimumColumnWidth = requiredWidth;
                }
            }
            
            __result = Mathf.Round(MinimumColumnWidth);
        }


    }

}
