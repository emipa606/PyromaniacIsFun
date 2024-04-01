#nullable enable
using HarmonyLib;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(Verb_LaunchProjectile))]
public static class Patch_Verb_LaunchProjectile_Projectile
{
    public static bool AllowFireArrow;
    public static NeedPyromania? Need;

    [HarmonyPrefix]
    [HarmonyPatch("TryCastShot")]
    public static void Prefix_TryCastShot(Verb_LaunchProjectile __instance)
    {
        if (__instance.caster is not Pawn pawn || !pawn.IsPyromaniac()
                                               || !Rand.Chance(0.6f))
        {
            return;
        }

        if (pawn.needs.TryGetNeed<NeedPyromania>() is { } need)
        {
            if (!(need.CurLevel > Patcher.Settings.NeedPyromaniaPerFireArrow))
            {
                return;
            }

            AllowFireArrow = true;
            Need = need;
        }
        else
        {
            AllowFireArrow = true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Verb_LaunchProjectile.Projectile), MethodType.Getter)]
    public static void Postfix_Projectile(ref ThingDef __result)
    {
        // Patch arrows in all `.Projectile` access in `.TryCastShot`.
        // In vanilla only the first one is needed, but the sequence might change due to mod.
        if (AllowFireArrow && PyromaniacUtility.ArrowDict.TryGetValue(__result, out var fireArrow))
        {
            __result = fireArrow;
        }
        else
        {
            // Reset it if no fire arrow is shot
            Need = null;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch("TryCastShot")]
    public static void Postfix_TryCastShot()
    {
        Need?.AdjustExternally(-Patcher.Settings.NeedPyromaniaPerFireArrow);
        // reset
        AllowFireArrow = false;
        Need = null;
    }
}