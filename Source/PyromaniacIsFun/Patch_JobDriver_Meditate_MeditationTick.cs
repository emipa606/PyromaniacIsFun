using HarmonyLib;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(JobDriver_Meditate), "MeditationTick")]
public static class Patch_JobDriver_Meditate_MeditationTick
{
    public static void Postfix(JobDriver_Meditate __instance)
    {
        var pawn = __instance.pawn;
        // See `Pawn_PsychicEntropyTracker.GainPsyfocus`
        if (__instance.Focus.Thing is not ThingWithComps thing || thing.Destroyed
                                                               || pawn.needs.TryGetNeed<NeedPyromania>() is not { } need
                                                               || thing.def
                                                                       .GetCompProperties<
                                                                           CompProperties_MeditationFocus>()
                                                                   is
                                                                   not { } comp
                                                               || !comp.focusTypes.Any(focusDef =>
                                                                   focusDef == PyromaniacUtility.FocusFlameDef))
        {
            return;
        }

        var valuePerDay = thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn) *
                          Patcher.Settings.NeedPyromaniaGainFromMeditationMultiplier;
        need.AdjustExternally(valuePerDay / GenDate.TicksPerDay,
            "CF_PyromaniacIsFun_NeedPyromania.WatchingFlame".Translate((valuePerDay * 100).ToString("F0"),
                (valuePerDay * 100 / GenDate.HoursPerDay).ToString("F0")));
    }
}