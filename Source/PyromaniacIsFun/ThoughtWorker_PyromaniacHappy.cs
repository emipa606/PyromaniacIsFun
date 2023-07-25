using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

public class ThoughtWorker_PyromaniacHappy : ThoughtWorker
{
    // Modified from ThoughtWorker_IsCarryingIncendiaryWeapon
    // Do not generate thought for e.g. `Gun_SmokeLauncher`
    protected override ThoughtState CurrentStateInternal(Pawn p)
    {
        if (p.equipment.Primary is null)
        {
            return false;
        }

        if (Patcher.Settings.HappyWhenCarryingTrulyIncendiaryWeapon)
        {
            // TODO: This is the standard way to get verbs from an equipment
            foreach (var verb in p.equipment.Primary.GetComp<CompEquippable>().AllVerbs)
            {
                // If it is loadable (only mortar in vanilla), get the loaded projectile
                if (verb.GetProjectile()?.projectile.damageDef == DamageDefOf.Flame)
                {
                    return true;
                }
            }
        }
        else
        {
            // Original
            foreach (var verb in p.equipment.Primary.GetComp<CompEquippable>().AllVerbs)
            {
                if (verb.IsIncendiary_Melee() || verb.IsIncendiary_Ranged())
                {
                    return true;
                }
            }
        }

        return false;
    }
}