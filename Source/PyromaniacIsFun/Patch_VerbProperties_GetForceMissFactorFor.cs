#nullable enable

using HarmonyLib;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(VerbProperties), nameof(VerbProperties.GetForceMissFactorFor))]
public static class Patch_VerbProperties_GetForceMissFactorFor
{
    public static void Postfix(VerbProperties __instance, Thing equipment, Pawn caster, ref float __result)
    {
        if (!Patcher.Settings.RemoveForcedMissRadius || __instance.defaultProjectile?.projectile.damageDef !=
                                                     DamageDefOf.Flame
                                                     || !equipment.def.IsRangedWeapon
                                                     || !caster.IsPyromaniac())
        {
            return;
        }

        __result = 0;
        PyromaniacUtility.ThrowText(caster,
            () => "CF_PyromaniacIsFun_PyromaniacUtility_ReducedForcedMissedRadius".Translate(caster,
                equipment.def.label), 4);
    }
}