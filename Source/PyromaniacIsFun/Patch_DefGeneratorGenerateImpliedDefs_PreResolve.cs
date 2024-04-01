#nullable enable

using HarmonyLib;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

[HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PreResolve))]
public static class Patch_DefGeneratorGenerateImpliedDefs_PreResolve
{
    public static ThingDef GenerateFireArrow(ThingDef arrow, ThingDef fireArrowGeneric)
    {
        var fireArrow = Gen.MemberwiseClone(arrow);
        fireArrow.defName = "Fire_" + arrow.defName;
        fireArrow.label = "CF_PyromaniacIsFun_FireArrows.WithFire".Translate(arrow.label);
        fireArrow.graphicData = fireArrowGeneric.graphicData;
        // Make a copy in order to modify
        // TODO: Any better to clone?
        fireArrow.projectile = Gen.MemberwiseClone(arrow.projectile);
        fireArrow.projectile.extraDamages = [];
        if (arrow.projectile.extraDamages is not null)
        {
            fireArrow.projectile.extraDamages.AddRange(arrow.projectile.extraDamages);
        }

        if (fireArrowGeneric.projectile.extraDamages is not null)
        {
            fireArrow.projectile.extraDamages.AddRange(fireArrowGeneric.projectile.extraDamages);
        }

        return fireArrow;
    }


    public static void Postfix()
    {
        foreach (var t in DefDatabase<ThingDef>.AllDefs)
        {
            if (t.projectile?.damageDef is not { defName: "Arrow" or "ArrowHighVelocity" })
            {
                continue;
            }

            var fireArrow = GenerateFireArrow(t, PyromaniacUtility.FireArrowGenericDef);
            PyromaniacUtility.ArrowDict.Add(t, fireArrow);
        }

        foreach (var fireArrow in PyromaniacUtility.ArrowDict.Values)
        {
            DefGenerator.AddImpliedDef(fireArrow);
        }
    }
}