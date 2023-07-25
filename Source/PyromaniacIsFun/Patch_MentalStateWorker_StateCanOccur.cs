using HarmonyLib;
using Verse;
using Verse.AI;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(MentalStateWorker))]
[HarmonyPatch(nameof(MentalStateWorker.StateCanOccur))]
public static class Patch_MentalStateWorker_StateCanOccur
{
    public static void Postfix(MentalStateWorker __instance, Pawn pawn, ref bool __result)
    {
        if (!__result || __instance.def != PyromaniacUtility.FireStartingSpreeDef
                      || pawn.needs.TryGetNeed<NeedPyromania>() is not { } need
                      || !(pawn.mindState.mentalBreaker.CurMood > need.GetMentalBreakProtectThreshold()))
        {
            return;
        }

        __result = false;
        PyromaniacUtility.ThrowText(pawn,
            () => "CF_PyromaniacIsFun_PyromaniacUtility_PreventedFireStartingSpree".Translate(), 4);
    }
}