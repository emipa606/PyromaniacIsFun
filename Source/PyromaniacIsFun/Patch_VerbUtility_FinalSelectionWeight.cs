#nullable enable

using HarmonyLib;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(VerbUtility), nameof(VerbUtility.FinalSelectionWeight))]
public static class Patch_VerbUtility_FinalSelectionWeight
{
    public static void Postfix(Verb verb, Pawn p, ref float __result)
    {
        if (!p.IsPyromaniac())
        {
            return;
        }

        var need = p.needs.TryGetNeed<NeedPyromania>();
        if (need is not null && !(need.CurLevel > Patcher.Settings.NeedPyromaniaPerIgnite))
        {
            return;
        }

        if (verb.maneuver != PyromaniacUtility.IgniteDef)
        {
            return;
        }

        var chance = Patcher.Settings.MeleeIgniteChance;
        // Assume original weight = 0, and all weights sum to 1
        // In fact they don't add to 1 often
        // weight / (1 + weight)
        __result = chance / (1 - chance);
    }
}