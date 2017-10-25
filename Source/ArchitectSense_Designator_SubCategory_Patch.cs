using Verse;

namespace StuffCount
{
    static class ArchitectSense_Designator_SubCategory_ShowOptions_Patch
    {
        public static void Prefix()
        {
            FloatMenu floatMenu = Find.WindowStack.FloatMenu;
            if (floatMenu != null&&!floatMenu.GetType().IsSubclassOf(typeof(FloatMenu)))
            {
                floatMenu.Close(false);
            }
        }
    }
}
