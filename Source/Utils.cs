using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace StuffCount
{
    public static class Utils
    {

        public static int Count(ThingDef tDef, Map map)
        {
            return tDef.CountAsResource
                ? map.resourceCounter.GetCount(tDef)
                : map.listerThings.ThingsOfDef(tDef).Count;
        }

        public static void ShowInfo(ThingDef tDef)
        {
            if (!CanShowInfo(tDef))
            {
                Messages.Message("Can't show info for this.",MessageSound.RejectInput);
                return;
            }

            if (tDef.thingClass == typeof(MinifiedThing))
            {
                List<ThingDef> list = (from def in DefDatabase<ThingDef>.AllDefs
                                                   where def.minifiedDef == tDef
                                                   select def).ToList();
                if (list.Count == 1) tDef = list[0];
                else
                {
                    Messages.Message("Can't show info for this.", MessageSound.RejectInput);
                    return;
                }
            }

            if (tDef.MadeFromStuff)
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();

                foreach (var sDef in GenStuff.AllowedStuffsFor(tDef))
                {
                    int? stuffCount = null;
                    if (Find.VisibleMap != null)
                        stuffCount = Count(sDef, Find.VisibleMap);
                    string labelCap = sDef.LabelCap + " (" + (stuffCount?.ToString() ?? "?") + ")";

                    list.Add(new FloatMenuOption(labelCap, delegate
                    {
                        Vector2 scrollPosition = new Vector2();
                        bool restoreScrollPos = false;
                        if (Find.WindowStack.IsOpen<Dialog_InfoCard>())
                        {
                            scrollPosition = (Vector2)AccessTools.Field(typeof(StatsReportUtility), "scrollPosition").GetValue(null);
                            restoreScrollPos = true;
                        }

                        Find.WindowStack.Add(new Dialog_InfoCard(FakeThingForInfo(tDef, sDef)));

                        if (restoreScrollPos)
                        {
                            AccessTools.Field(typeof(StatsReportUtility), "scrollPosition").SetValue(null, scrollPosition);
                        }
                    },
                        MenuOptionPriority.Default, null, null, 0f, null, null));
                }


                FloatMenu floatMenu = new FloatMenu(list);
                floatMenu.vanishIfMouseDistant = true;
                Find.WindowStack.Add(floatMenu);
            }
            else
            {
                Find.WindowStack.Add(new Dialog_InfoCard(FakeThingForInfo(tDef, null)));
            }


        }

        private static Thing FakeThingForInfo(ThingDef tDef, ThingDef stuff)
        {
            return ThingMaker.MakeThing(tDef, stuff);
        }

        static readonly MethodInfo infoCardButtonWorker = AccessTools.Method(typeof(Widgets), "InfoCardButtonWorker");
        public static bool MyInfoCardButton(float x, float y, ThingDef tDef, ref bool result)
        {
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))&&CanShowInfo(tDef))
            {
                bool flag = (bool)infoCardButtonWorker.Invoke(null, new object[] { x, y });
                Widgets.DrawHighlight(new Rect(x, y, 24f, 24f));
                if (flag)
                {
                    ShowInfo(tDef);
                    result = true;
                }
                else
                {
                    result = false;
                }
                return false;
            }
            return true;
        }

        public static bool CanShowInfo(ThingDef def)
        {
            return (
                def.category != ThingCategory.Pawn &&
                def.thingClass != typeof(Corpse) &&
                !def.IsBlueprint &&
                !def.IsFrame &&
                def != ThingDefOf.ActiveDropPod &&
                //def.thingClass != typeof(MinifiedThing) && 
                def.thingClass != typeof(UnfinishedThing) &&
                !def.destroyOnDrop &&
                (def.category == ThingCategory.Item ||def.category == ThingCategory.Building)
                );
        }
    }
}
