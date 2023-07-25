using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace CF_PyromaniacIsFun;

public static class PyromaniacUtility
{
    public static readonly ThoughtDef ObservedWildFireDef =
        DefDatabase<ThoughtDef>.GetNamed("CF_PyromaniacIsFun_ObservedWildFire");

    public static readonly ThoughtDef ObservedBurningPawnDef =
        DefDatabase<ThoughtDef>.GetNamed("CF_PyromaniacIsFun_ObservedBurningPawn");

    public static readonly ThoughtDef SelfOnFireDef = DefDatabase<ThoughtDef>.GetNamed("CF_PyromaniacIsFun_SelfOnFire");

    public static readonly ThingDef FireArrowGenericDef =
        DefDatabase<ThingDef>.GetNamed("CF_PyromaniacIsFun_FireArrowTemplate");

    public static readonly NeedDef NeedPyromaniaDef = DefDatabase<NeedDef>.GetNamed("CF_PyromaniacIsFun_NeedPyromania");
    public static readonly MeditationFocusDef FocusFlameDef = DefDatabase<MeditationFocusDef>.GetNamed("Flame");
    public static readonly ManeuverDef IgniteDef = DefDatabase<ManeuverDef>.GetNamed("CF_PyromaniacIsFun_Ignite");

    public static readonly MentalStateDef FireStartingSpreeDef =
        DefDatabase<MentalStateDef>.GetNamed("FireStartingSpree");

    public static readonly Dictionary<ThingDef, ThingDef> ArrowDict = new Dictionary<ThingDef, ThingDef>();
    public static bool IsDebug;

    [DebugAction("PyromaniacIsFun", actionType = DebugActionType.Action,
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void ToggleDebug()
    {
        IsDebug = !IsDebug;
        Log.Message($"Debug is {IsDebug}");
    }

    [DebugAction("PyromaniacIsFun", actionType = DebugActionType.ToolMapForPawns,
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void LogPyromaniacThoughts(Pawn pawn)
    {
        if (pawn.needs.mood?.thoughts.memories is not { } handler)
        {
            return;
        }

        foreach (var thought in handler.Memories)
        {
            if (thought.def == ObservedWildFireDef)
            {
                Log.Message($"{thought}");
            }
            else if (thought.def == ObservedBurningPawnDef)
            {
                Log.Message($"{thought}");
            }
        }
    }

    [DebugAction("PyromaniacIsFun", actionType = DebugActionType.Action,
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void LogArrowDict()
    {
        foreach (var (k, v) in ArrowDict)
        {
            Log.Message($"ArrowDict: {k} => {v}");
        }
    }

    [DebugAction("PyromaniacIsFun", actionType = DebugActionType.Action,
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void LogVanillaIncendiaryProjectile()
    {
        foreach (var thingDef in DefDatabase<ThingDef>.AllDefs)
        {
            if (thingDef.projectile?.ai_IsIncendiary ?? false)
            {
                Log.Message($"Incendiary projectile: {thingDef}");
            }
        }
    }

    [DebugAction("PyromaniacIsFun", actionType = DebugActionType.ToolMapForPawns,
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void LogBattleEntry(Pawn pawn)
    {
        foreach (var battle in Find.BattleLog.Battles)
        {
            if (!battle.Concerns(pawn))
            {
                continue;
            }

            foreach (var entry in battle.Entries)
            {
                if (!entry.Concerns(pawn))
                {
                    continue;
                }

                Log.Message($"{battle.GetName()}: {entry}");
                Log.Message($"{entry.ToGameStringFromPOV(pawn)}");
            }
        }
    }

    [DebugAction("PyromaniacIsFun", actionType = DebugActionType.ToolMapForPawns,
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void RemoveBattleEntry(Pawn pawn)
    {
        var n = 0;
        foreach (var battle in Find.BattleLog.Battles)
        {
            if (!battle.Concerns(pawn))
            {
                continue;
            }

            n += battle.Entries.RemoveAll(entry => entry.Concerns(pawn));
        }

        MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, $"Removed {n} entries");
    }

    [DebugAction("PyromaniacIsFun", actionType = DebugActionType.ToolMapForPawns,
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void TriggerPyromaniaNeedInterval(Pawn pawn)
    {
        pawn.needs.TryGetNeed<NeedPyromania>()?.NeedInterval();
    }

    [DebugAction("PyromaniacIsFun", actionType = DebugActionType.ToolMapForPawns,
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void IncreaseNeedPyromaniaBy10(Pawn pawn)
    {
        pawn.needs.TryGetNeed<NeedPyromania>()?.AdjustExternally(0.1f);
    }

    [DebugAction("PyromaniacIsFun", actionType = DebugActionType.ToolMapForPawns,
        allowedGameStates = AllowedGameStates.PlayingOnMap)]
    public static void DecreaseNeedPyromaniaBy10(Pawn pawn)
    {
        pawn.needs.TryGetNeed<NeedPyromania>()?.AdjustExternally(-0.1f);
    }

    public static void ThrowText(Thing thing, Func<string> GetText, float timeBeforeStartFadeout)
    {
        if (IsDebug)
        {
            MoteMaker.ThrowText(thing.DrawPos, thing.Map, GetText(), timeBeforeStartFadeout);
        }
    }

    public static bool IsPyromaniac(this Pawn pawn)
    {
        return pawn.story?.traits.HasTrait(TraitDefOf.Pyromaniac) ?? false;
    }
}