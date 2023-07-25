using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(ShotReport))]
[HarmonyPatch(nameof(ShotReport.HitReportFor))]
public static class Patch_ShotReport_HitReportFor
{
    public static readonly FieldInfo ShotReport_forcedMissRadius =
        typeof(ShotReport).GetField("forcedMissRadius", BindingFlags.Instance | BindingFlags.NonPublic) ??
        throw new ArgumentException("ShotReport.forcedMissRadius is not found");

    public static void Postfix(ref ShotReport __result, Thing caster, Verb verb)
    {
        if (!Patcher.Settings.RemoveForcedMissRadius || caster is not Pawn pawn ||
            verb.GetProjectile()?.projectile.damageDef != DamageDefOf.Flame
            || !pawn.IsPyromaniac())
        {
            return;
        }

        // TODO: This involves copy, not ideal
        object r = __result;
        ShotReport_forcedMissRadius.SetValue(r, 0);
        __result = (ShotReport)r;
    }
}