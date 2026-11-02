using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(ShotReport), nameof(ShotReport.HitReportFor))]
public static class ShotReport_HitReportFor
{
    private static readonly FieldInfo shotReportForcedMissRadius =
        AccessTools.Field(typeof(ShotReport), "forcedMissRadius");

    public static void Postfix(ref ShotReport __result, Thing caster, Verb verb)
    {
        if (!Patcher.Settings.RemoveForcedMissRadius || caster is not Pawn pawn ||
            verb.GetProjectile()?.projectile.damageDef != DamageDefOf.Flame
            || !pawn.IsPyromaniac())
        {
            return;
        }

        object r = __result;
        shotReportForcedMissRadius.SetValue(r, 0);
        __result = (ShotReport)r;
    }
}