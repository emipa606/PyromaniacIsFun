using HarmonyLib;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(nameof(Verb_LaunchProjectile.Projectile), MethodType.Getter)]
public static class Verb_LaunchProjectile_Projectile
{
    public static void Postfix_Projectile(ref ThingDef __result)
    {
        // Patch arrows in all `.Projectile` access in `.TryCastShot`.
        // In vanilla only the first one is needed, but the sequence might change due to mod.
        if (Verb_LaunchProjectile_TryCastShot.AllowFireArrow &&
            PyromaniacUtility.ArrowDict.TryGetValue(__result, out var fireArrow))
        {
            __result = fireArrow;
        }
        else
        {
            // Reset it if no fire arrow is shot
            Verb_LaunchProjectile_TryCastShot.Need = null;
        }
    }
}