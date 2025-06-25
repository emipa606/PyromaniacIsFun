#nullable enable
using HarmonyLib;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(Verb_LaunchProjectile), "TryCastShot")]
public static class Verb_LaunchProjectile_TryCastShot
{
    public static bool AllowFireArrow;
    public static NeedPyromania? Need;

    public static void Prefix(Verb_LaunchProjectile __instance)
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

    public static void Postfix()
    {
        Need?.AdjustExternally(-Patcher.Settings.NeedPyromaniaPerFireArrow);
        // reset
        AllowFireArrow = false;
        Need = null;
    }
}