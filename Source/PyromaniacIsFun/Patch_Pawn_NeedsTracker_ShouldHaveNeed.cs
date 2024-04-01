using HarmonyLib;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
public static class Patch_Pawn_NeedsTracker_ShouldHaveNeed
{
    public static void Postfix(NeedDef nd, ref bool __result, Pawn ___pawn)
    {
        if (!__result || nd != PyromaniacUtility.NeedPyromaniaDef)
        {
            return;
        }

        if (!ModsConfig.RoyaltyActive || !___pawn.IsPyromaniac())
        {
            // Disable if Royalty DLC not installed, or a pawn is not pyromaniac
            __result = false;
        }
    }
}