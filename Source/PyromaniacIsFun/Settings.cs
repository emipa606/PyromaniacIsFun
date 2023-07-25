using Verse;

namespace CF_PyromaniacIsFun;

public class Settings : ModSettings
{
    public bool HappyWhenCarryingTrulyIncendiaryWeapon = true;
    public float MeleeIgniteChance = 0.4f;
    public float NeedPyromaniaGainFromMeditationMultiplier = 4f;
    public float NeedPyromaniaGainPerBurningPawnPerDay = 0.3f;
    public float NeedPyromaniaGainPerWildFirePerDay = 0.04f;
    public float NeedPyromaniaGainSelfOnFirePerDay = 0.6f;
    public float NeedPyromaniaPerFireArrow = 0.02f;
    public float NeedPyromaniaPerIgnite = 0.02f;
    public bool RemoveForcedMissRadius = true;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref NeedPyromaniaPerFireArrow, nameof(NeedPyromaniaPerFireArrow), 0.02f);
        Scribe_Values.Look(ref NeedPyromaniaPerIgnite, nameof(NeedPyromaniaPerIgnite), 0.02f);
        Scribe_Values.Look(ref MeleeIgniteChance, nameof(MeleeIgniteChance), 0.4f);
        Scribe_Values.Look(ref NeedPyromaniaGainPerWildFirePerDay, nameof(NeedPyromaniaGainPerWildFirePerDay), 0.04f);
        Scribe_Values.Look(ref NeedPyromaniaGainPerBurningPawnPerDay, nameof(NeedPyromaniaGainPerBurningPawnPerDay),
            0.3f);
        Scribe_Values.Look(ref NeedPyromaniaGainSelfOnFirePerDay, nameof(NeedPyromaniaGainSelfOnFirePerDay), 0.6f);
        Scribe_Values.Look(ref NeedPyromaniaGainFromMeditationMultiplier,
            nameof(NeedPyromaniaGainFromMeditationMultiplier), 4f);
        Scribe_Values.Look(ref HappyWhenCarryingTrulyIncendiaryWeapon, nameof(HappyWhenCarryingTrulyIncendiaryWeapon),
            true);
        Scribe_Values.Look(ref RemoveForcedMissRadius, nameof(RemoveForcedMissRadius), true);
        base.ExposeData();
    }
}