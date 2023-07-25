using RimWorld;

namespace CF_PyromaniacIsFun;

internal class Verb_MeleeAttackDamageIgnite : Verb_MeleeAttackDamage
{
    protected override bool TryCastShot()
    {
        // TODO: Fire icon
        if (CasterPawn?.needs.TryGetNeed<NeedPyromania>() is { } need)
        {
            need.AdjustExternally(-Patcher.Settings.NeedPyromaniaPerIgnite);
        }

        // Enemies don't consume NeedPyromania
        return base.TryCastShot();
    }
}